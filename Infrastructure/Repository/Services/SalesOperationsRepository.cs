using Crosscutting.CustomException;
using Crosscutting.DTO.External;
using Crosscutting.Enum;
using Crosscutting.External;
using Crosscutting.Helpers;
using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repository.Services
{
    public class SalesOperationsRepository : ISalesOperationsRepository
    {
        private readonly DatabaseContext _context;
        private readonly IExternalBillingService _externalBillingService;

        public SalesOperationsRepository(DatabaseContext context, IExternalBillingService externalBillingService)
        {
            _context = context;
            _externalBillingService = externalBillingService;
        }

        public async Task ConfirmPurchaseEntryAsync(int entryId, string userEmail, DateTime now, CancellationToken ct = default)
        {
            await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(ct);

            try
            {
                ProductPurchaseEntryEntity? entry = await _context.Set<ProductPurchaseEntryEntity>()
                    .Include(e => e.Items)
                    .FirstOrDefaultAsync(e => e.Id == entryId, ct);

                if (entry == null)
                    throw new CustomBusinessException("Purchase entry not found.");

                if (entry.Status == PurchaseEntryStatusEnum.Confirmed)
                    throw new CustomBusinessException("Purchase entry is already confirmed.");

                if (!entry.Items.Any())
                    throw new CustomBusinessException("Purchase entry must contain at least one item.");

                if (string.IsNullOrWhiteSpace(entry.Number))
                    entry.Number = await GetNextNumberAsync(DocumentTypeEnum.PurchaseEntry, now.Year, userEmail, now, ct);

                foreach (ProductPurchaseEntryItemEntity item in entry.Items)
                {
                    ProductEntity? product = await _context.Set<ProductEntity>()
                        .FirstOrDefaultAsync(p => p.Id == item.ProductId, ct);

                    if (product == null)
                        throw new CustomBusinessException($"Product {item.ProductId} not found.");

                    decimal? previousMaxCost = await GetMaxUnitCostByProductIdAsync(item.ProductId, entryId, ct);
                    decimal costPrice = Math.Max(previousMaxCost ?? 0m, item.UnitCost);

                    product.CostPrice = costPrice;
                    product.MarkupPercent = item.MarkupPercent;
                    product.UnitPrice = SalesCalculationHelper.CalculateUnitPriceFromCost(costPrice, item.MarkupPercent);
                    product.UpdatedAt = now;
                    product.UpdatedBy = userEmail;

                    item.CalculatedUnitPrice = SalesCalculationHelper.CalculateUnitPriceFromCost(item.UnitCost, item.MarkupPercent);
                    item.TotalCost = item.UnitCost * item.Quantity;
                    item.UpdatedAt = now;
                    item.UpdatedBy = userEmail;

                    ProductStockEntity stock = await GetOrCreateStockAsync(item.ProductId, userEmail, now, ct);
                    stock.Quantity += item.Quantity;
                    stock.UpdatedAt = now;
                    stock.UpdatedBy = userEmail;

                    await _context.Set<StockMovementEntity>().AddAsync(new StockMovementEntity
                    {
                        ProductId = item.ProductId,
                        MovementType = StockMovementTypeEnum.PurchaseIn,
                        Quantity = item.Quantity,
                        BalanceAfter = stock.Quantity,
                        ProductPurchaseEntryItemId = item.Id,
                        OriginDocumentNumber = entry.Number,
                        Notes = $"Purchase entry {entry.Number}",
                        CreatedAt = now,
                        CreatedBy = userEmail
                    }, ct);
                }

                entry.Status = PurchaseEntryStatusEnum.Confirmed;
                entry.UpdatedAt = now;
                entry.UpdatedBy = userEmail;

                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }

        public async Task ConfirmStockAdjustmentAsync(int adjustmentId, string userEmail, DateTime now, CancellationToken ct = default)
        {
            await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(ct);

            try
            {
                StockAdjustmentEntity? adjustment = await _context.Set<StockAdjustmentEntity>()
                    .Include(a => a.Items)
                    .FirstOrDefaultAsync(a => a.Id == adjustmentId, ct);

                if (adjustment == null)
                    throw new CustomBusinessException("Stock adjustment not found.");

                if (adjustment.IsConfirmed)
                    throw new CustomBusinessException("Stock adjustment is already confirmed.");

                if (!adjustment.Items.Any())
                    throw new CustomBusinessException("Stock adjustment must contain at least one item.");

                if (string.IsNullOrWhiteSpace(adjustment.Number))
                    adjustment.Number = await GetNextNumberAsync(DocumentTypeEnum.StockAdjustment, now.Year, userEmail, now, ct);

                foreach (StockAdjustmentItemEntity item in adjustment.Items)
                {
                    if (item.Quantity == 0)
                        continue;

                    ProductEntity? product = await _context.Set<ProductEntity>()
                        .FirstOrDefaultAsync(p => p.Id == item.ProductId, ct);

                    if (product == null)
                        throw new CustomBusinessException($"Product {item.ProductId} not found.");

                    ProductStockEntity stock = await GetOrCreateStockAsync(item.ProductId, userEmail, now, ct);

                    if (item.Quantity < 0 && stock.Quantity + item.Quantity < 0)
                        throw new CustomBusinessException($"Insufficient stock for product {product.Name}.");

                    stock.Quantity += item.Quantity;
                    stock.UpdatedAt = now;
                    stock.UpdatedBy = userEmail;

                    StockMovementTypeEnum movementType = item.Quantity > 0
                        ? StockMovementTypeEnum.AdjustmentIn
                        : StockMovementTypeEnum.AdjustmentOut;

                    await _context.Set<StockMovementEntity>().AddAsync(new StockMovementEntity
                    {
                        ProductId = item.ProductId,
                        MovementType = movementType,
                        Quantity = Math.Abs(item.Quantity),
                        BalanceAfter = stock.Quantity,
                        StockAdjustmentItemId = item.Id,
                        OriginDocumentNumber = adjustment.Number,
                        Notes = item.Notes ?? adjustment.Reason,
                        CreatedAt = now,
                        CreatedBy = userEmail
                    }, ct);

                    item.UpdatedAt = now;
                    item.UpdatedBy = userEmail;
                }

                adjustment.IsConfirmed = true;
                adjustment.UpdatedAt = now;
                adjustment.UpdatedBy = userEmail;

                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }

        public async Task ConfirmSaleAsync(int saleId, string userEmail, DateTime now, CancellationToken ct = default)
        {
            await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(ct);

            try
            {
                SaleEntity? sale = await _context.Set<SaleEntity>()
                    .Include(s => s.Items)
                    .FirstOrDefaultAsync(s => s.Id == saleId, ct);

                if (sale == null)
                    throw new CustomBusinessException("Sale not found.");

                if (sale.Status == SaleStatusEnum.Confirmed)
                    throw new CustomBusinessException("Sale is already confirmed.");

                if (!sale.Items.Any())
                    throw new CustomBusinessException("Sale must contain at least one item.");

                foreach (SaleItemEntity item in sale.Items)
                {
                    ProductStockEntity stock = await GetOrCreateStockAsync(item.ProductId, userEmail, now, ct);

                    if (stock.Quantity < item.Quantity)
                    {
                        ProductEntity? product = await _context.Set<ProductEntity>()
                            .FirstOrDefaultAsync(p => p.Id == item.ProductId, ct);

                        throw new CustomBusinessException(
                            $"Insufficient stock for product {product?.Name ?? item.ProductId.ToString()}.");
                    }

                    stock.Quantity -= item.Quantity;
                    stock.UpdatedAt = now;
                    stock.UpdatedBy = userEmail;

                    await _context.Set<StockMovementEntity>().AddAsync(new StockMovementEntity
                    {
                        ProductId = item.ProductId,
                        MovementType = StockMovementTypeEnum.SaleOut,
                        Quantity = item.Quantity,
                        BalanceAfter = stock.Quantity,
                        SaleId = sale.Id,
                        OriginDocumentNumber = sale.Number,
                        Notes = $"Sale {sale.Number}",
                        CreatedAt = now,
                        CreatedBy = userEmail
                    }, ct);
                }

                CommissionRuleEntity? globalRule = await _context.Set<CommissionRuleEntity>()
                    .FirstOrDefaultAsync(r => r.ProductId == null && r.IsActive, ct);

                if (globalRule == null)
                    throw new CustomBusinessException("No active global commission rule found.");

                if (globalRule.CalculationScope == null)
                    throw new CustomBusinessException("Global commission rule must define a calculation scope.");

                CommissionCalculationScopeEnum calculationScope = globalRule.CalculationScope.Value;
                decimal totalCommissionAmount = 0m;

                if (calculationScope == CommissionCalculationScopeEnum.PerSale)
                {
                    decimal baseAmount = sale.Items.Sum(i => i.LineSubtotal);
                    decimal commissionAmount = SalesCalculationHelper.CalculateCommissionAmount(baseAmount, globalRule.CommissionPercent);

                    await _context.Set<CommissionEntity>().AddAsync(new CommissionEntity
                    {
                        SaleId = sale.Id,
                        SaleItemId = null,
                        CalculationScope = CommissionCalculationScopeEnum.PerSale,
                        SellerEmail = sale.SellerEmail,
                        CommissionPercent = globalRule.CommissionPercent,
                        BaseAmount = baseAmount,
                        CommissionAmount = commissionAmount,
                        Status = CommissionStatusEnum.Pending,
                        CreatedAt = now,
                        CreatedBy = userEmail
                    }, ct);

                    totalCommissionAmount = commissionAmount;
                }
                else
                {
                    foreach (SaleItemEntity item in sale.Items)
                    {
                        CommissionRuleEntity? productRule = await _context.Set<CommissionRuleEntity>()
                            .FirstOrDefaultAsync(r => r.ProductId == item.ProductId && r.IsActive, ct);

                        decimal commissionPercent = productRule?.CommissionPercent ?? globalRule.CommissionPercent;
                        decimal baseAmount = item.LineSubtotal;
                        decimal commissionAmount = SalesCalculationHelper.CalculateCommissionAmount(baseAmount, commissionPercent);

                        await _context.Set<CommissionEntity>().AddAsync(new CommissionEntity
                        {
                            SaleId = sale.Id,
                            SaleItemId = item.Id,
                            CalculationScope = CommissionCalculationScopeEnum.PerItem,
                            SellerEmail = sale.SellerEmail,
                            CommissionPercent = commissionPercent,
                            BaseAmount = baseAmount,
                            CommissionAmount = commissionAmount,
                            Status = CommissionStatusEnum.Pending,
                            CreatedAt = now,
                            CreatedBy = userEmail
                        }, ct);

                        totalCommissionAmount += commissionAmount;
                    }
                }

                sale.CommissionAmount = totalCommissionAmount;
                sale.ExternalBillingId = await _externalBillingService.CreateBillingAsync(new CreateExternalBillingRequestDTO
                {
                    SaleId = sale.Id,
                    ExternalClientId = sale.ExternalClientId,
                    TotalAmount = sale.TotalAmount,
                    SellerEmail = sale.SellerEmail,
                    SaleNumber = sale.Number
                }, ct);
                sale.Status = SaleStatusEnum.Confirmed;
                sale.SoldAt = now;
                sale.UpdatedAt = now;
                sale.UpdatedBy = userEmail;

                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }

        private async Task<ProductStockEntity> GetOrCreateStockAsync(int productId, string userEmail, DateTime now, CancellationToken ct)
        {
            ProductStockEntity? stock = await _context.Set<ProductStockEntity>()
                .FirstOrDefaultAsync(s => s.ProductId == productId, ct);

            if (stock != null)
                return stock;

            stock = new ProductStockEntity
            {
                ProductId = productId,
                Quantity = 0,
                CreatedAt = now,
                CreatedBy = userEmail
            };

            await _context.Set<ProductStockEntity>().AddAsync(stock, ct);
            return stock;
        }

        private async Task<decimal?> GetMaxUnitCostByProductIdAsync(int productId, int excludeEntryId, CancellationToken ct)
        {
            return await _context.Set<ProductPurchaseEntryItemEntity>()
                .Join(
                    _context.Set<ProductPurchaseEntryEntity>(),
                    item => item.ProductPurchaseEntryId,
                    entry => entry.Id,
                    (item, entry) => new { item, entry })
                .Where(x => x.item.ProductId == productId &&
                            x.entry.Status == PurchaseEntryStatusEnum.Confirmed &&
                            x.entry.Id != excludeEntryId)
                .MaxAsync(x => (decimal?)x.item.UnitCost, ct);
        }

        private async Task<string> GetNextNumberAsync(DocumentTypeEnum type, int year, string userEmail, DateTime now, CancellationToken ct)
        {
            var set = _context.Set<DocumentSequenceEntity>();
            DocumentSequenceEntity? sequence = await set.FirstOrDefaultAsync(x => x.DocumentType == type && x.Year == year, ct);

            if (sequence == null)
            {
                sequence = new DocumentSequenceEntity
                {
                    DocumentType = type,
                    Year = year,
                    LastNumber = 0,
                    CreatedAt = now,
                    CreatedBy = userEmail
                };

                await set.AddAsync(sequence, ct);
            }

            sequence.LastNumber++;
            sequence.UpdatedAt = now;
            sequence.UpdatedBy = userEmail;

            return $"{year}/{sequence.LastNumber:D6}";
        }
    }
}
