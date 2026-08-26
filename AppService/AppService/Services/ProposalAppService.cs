using Crosscutting.External;
using AppService.AppService.Interfaces;
using AutoMapper;
using Crosscutting.CustomException;
using Crosscutting.DTO.Proposal;
using Crosscutting.DTO.Sale;
using Crosscutting.Enum;
using Crosscutting.Helpers;
using Domain.Entity;
using Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AppService.AppService.Services
{
    public class ProposalAppService : BaseService<IProposalRepository, ProposalEntity>, IProposalAppService
    {
        private readonly IExternalClientService _externalClientService;
        private readonly IProductRepository _productRepository;
        private readonly IProductStockRepository _productStockRepository;
        private readonly IDocumentSequenceRepository _documentSequenceRepository;
        private readonly ISalesOperationsRepository _salesOperationsRepository;
        private readonly ISaleRepository _saleRepository;

        public ProposalAppService(
            IMapper mapper,
            IProposalRepository repository,
            IExternalClientService externalClientService,
            IProductRepository productRepository,
            IProductStockRepository productStockRepository,
            IDocumentSequenceRepository documentSequenceRepository,
            ISalesOperationsRepository salesOperationsRepository,
            ISaleRepository saleRepository,
            IHttpContextAccessor httpContextAccessor) : base(mapper, repository, httpContextAccessor)
        {
            _externalClientService = externalClientService;
            _productRepository = productRepository;
            _productStockRepository = productStockRepository;
            _documentSequenceRepository = documentSequenceRepository;
            _salesOperationsRepository = salesOperationsRepository;
            _saleRepository = saleRepository;
        }

        public async Task<IReadOnlyList<ProposalResponseDTO>> GetAllAsync(CancellationToken ct = default)
        {
            IReadOnlyList<ProposalEntity> entities = await GetAllReadOnlyAsync(ct);
            return _mapper.Map<IReadOnlyList<ProposalResponseDTO>>(entities);
        }

        public async Task<ProposalResponseDTO> GetResponseByIdAsync(int id, CancellationToken ct = default)
        {
            ProposalEntity entity = await GetProposalWithItemsAsync(id, ct);
            return _mapper.Map<ProposalResponseDTO>(entity);
        }

        public async Task<ProposalResponseDTO> CreateAsync(CreateProposalRequestDTO request, CancellationToken ct = default)
        {
            await _externalClientService.ValidateClientAsync(request.ExternalClientId, ct);

            if (!request.Items.Any())
                throw new CustomBusinessException("Ops... A proposta deve conter ao menos um item.");

            DateTime now = GetCurrentDateTime();
            string userEmail = GetCurrentUserEmail();

            ProposalEntity entity = new()
            {
                ExternalClientId = request.ExternalClientId,
                SellerEmail = userEmail,
                PaymentConditionId = request.PaymentConditionId,
                SaleType = request.SaleType,
                Status = ProposalStatusEnum.Draft,
                ValidUntil = request.ValidUntil,
                Notes = request.Notes,
                CreatedAt = now,
                CreatedBy = userEmail
            };

            foreach (CreateProposalItemRequestDTO itemRequest in request.Items)
            {
                ProposalItemEntity item = await BuildProposalItemAsync(itemRequest, now, userEmail, ct);
                entity.Items.Add(item);
            }

            RecalculateProposalTotals(entity);
            await ApplyStockWarningsAsync(entity, ct);

            ProposalEntity createdEntity = await AddAsync(entity, ct);
            ProposalEntity entityWithItems = await GetProposalWithItemsAsync(createdEntity.Id, ct);
            return _mapper.Map<ProposalResponseDTO>(entityWithItems);
        }

        public async Task<ProposalResponseDTO> UpdateAsync(int id, UpdateProposalRequestDTO request, CancellationToken ct = default)
        {
            ProposalEntity entity = await GetProposalWithItemsAsync(id, ct);
            EnsureEditable(entity);

            await _externalClientService.ValidateClientAsync(request.ExternalClientId, ct);

            if (!request.Items.Any())
                throw new CustomBusinessException("Ops... A proposta deve conter ao menos um item.");

            DateTime now = GetCurrentDateTime();
            string userEmail = GetCurrentUserEmail();

            entity.ExternalClientId = request.ExternalClientId;
            entity.PaymentConditionId = request.PaymentConditionId;
            entity.SaleType = request.SaleType;
            entity.ValidUntil = request.ValidUntil;
            entity.Notes = request.Notes;
            entity.UpdatedAt = now;
            entity.UpdatedBy = userEmail;

            entity.Items.Clear();

            foreach (UpdateProposalItemRequestDTO itemRequest in request.Items)
            {
                ProposalItemEntity item = await BuildProposalItemAsync(itemRequest, now, userEmail, ct);
                entity.Items.Add(item);
            }

            RecalculateProposalTotals(entity);
            await ApplyStockWarningsAsync(entity, ct);

            ProposalEntity updatedEntity = await EditAsync(entity);
            ProposalEntity entityWithItems = await GetProposalWithItemsAsync(updatedEntity.Id, ct);
            return _mapper.Map<ProposalResponseDTO>(entityWithItems);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            ProposalEntity entity = await GetProposalWithItemsAsync(id, ct);
            EnsureEditable(entity);
            await DeleteAsync(entity);
        }

        public async Task<ProposalResponseDTO> SendAsync(int id, CancellationToken ct = default)
        {
            ProposalEntity entity = await GetProposalWithItemsAsync(id, ct);

            if (entity.Status != ProposalStatusEnum.Draft)
                throw new CustomBusinessException("Ops... Somente propostas em rascunho podem ser enviadas.");

            DateTime now = GetCurrentDateTime();
            string userEmail = GetCurrentUserEmail();

            if (string.IsNullOrWhiteSpace(entity.Number))
                entity.Number = await _documentSequenceRepository.GetNextNumberAsync(DocumentTypeEnum.Proposal, now.Year, ct);

            entity.Status = ProposalStatusEnum.Sent;
            entity.UpdatedAt = now;
            entity.UpdatedBy = userEmail;

            ProposalEntity updatedEntity = await EditAsync(entity);
            ProposalEntity entityWithItems = await GetProposalWithItemsAsync(updatedEntity.Id, ct);
            return _mapper.Map<ProposalResponseDTO>(entityWithItems);
        }

        public async Task<ProposalResponseDTO> ApproveAsync(int id, CancellationToken ct = default)
        {
            ProposalEntity entity = await GetProposalWithItemsAsync(id, ct);

            if (entity.Status != ProposalStatusEnum.Sent)
                throw new CustomBusinessException("Ops... Somente propostas enviadas podem ser aprovadas.");

            DateTime now = GetCurrentDateTime();
            string userEmail = GetCurrentUserEmail();

            entity.Status = ProposalStatusEnum.Approved;
            entity.UpdatedAt = now;
            entity.UpdatedBy = userEmail;

            ProposalEntity updatedEntity = await EditAsync(entity);
            ProposalEntity entityWithItems = await GetProposalWithItemsAsync(updatedEntity.Id, ct);
            return _mapper.Map<ProposalResponseDTO>(entityWithItems);
        }

        public async Task<SaleResponseDTO> ConvertToSaleAsync(int id, CancellationToken ct = default)
        {
            ProposalEntity proposal = await GetProposalWithItemsAsync(id, ct);

            if (proposal.Status != ProposalStatusEnum.Approved)
                throw new CustomBusinessException("Ops... Somente propostas aprovadas podem ser convertidas em venda.");

            if (!proposal.Items.Any())
                throw new CustomBusinessException("Ops... A proposta deve conter ao menos um item.");

            foreach (ProposalItemEntity item in proposal.Items)
            {
                ProductStockEntity? stock = await _productStockRepository.GetByProductIdAsync(item.ProductId, ct);
                decimal physical = stock?.Quantity ?? 0m;

                if (physical < item.Quantity)
                {
                    ProductEntity? product = await _productRepository.GetByIdAsync(item.ProductId);
                    throw new CustomBusinessException(
                        $"Ops... Estoque insuficiente para o produto {product?.Name ?? item.ProductId.ToString()}.");
                }
            }

            DateTime now = GetCurrentDateTime();
            string userEmail = GetCurrentUserEmail();

            SaleEntity sale = new()
            {
                ProposalId = proposal.Id,
                ExternalClientId = proposal.ExternalClientId,
                SellerEmail = proposal.SellerEmail,
                PaymentConditionId = proposal.PaymentConditionId,
                SaleType = proposal.SaleType,
                Status = SaleStatusEnum.Pending,
                SubtotalAmount = proposal.SubtotalAmount,
                DiscountAmount = proposal.DiscountAmount,
                TaxAmount = proposal.TaxAmount,
                TotalAmount = proposal.TotalAmount,
                Number = await _documentSequenceRepository.GetNextNumberAsync(DocumentTypeEnum.Sale, now.Year, ct),
                CreatedAt = now,
                CreatedBy = userEmail
            };

            foreach (ProposalItemEntity proposalItem in proposal.Items)
            {
                sale.Items.Add(new SaleItemEntity
                {
                    ProductId = proposalItem.ProductId,
                    Quantity = proposalItem.Quantity,
                    UnitPrice = proposalItem.UnitPrice,
                    DiscountPercent = proposalItem.DiscountPercent,
                    DiscountAmount = proposalItem.DiscountAmount,
                    LineSubtotal = proposalItem.LineSubtotal,
                    IcmsPercent = proposalItem.IcmsPercent,
                    IcmsAmount = proposalItem.IcmsAmount,
                    IssPercent = proposalItem.IssPercent,
                    IssAmount = proposalItem.IssAmount,
                    PisPercent = proposalItem.PisPercent,
                    PisAmount = proposalItem.PisAmount,
                    CofinsPercent = proposalItem.CofinsPercent,
                    CofinsAmount = proposalItem.CofinsAmount,
                    LineTotal = proposalItem.LineTotal,
                    CreatedAt = now,
                    CreatedBy = userEmail
                });
            }

            SaleEntity createdSale = await _saleRepository.AddAsync(sale, ct);
            await _salesOperationsRepository.ConfirmSaleAsync(createdSale.Id, userEmail, now, ct);

            proposal.Status = ProposalStatusEnum.Converted;
            proposal.UpdatedAt = now;
            proposal.UpdatedBy = userEmail;
            await EditAsync(proposal);

            SaleEntity confirmedSale = await _saleRepository.GetByIdWithDetailsAsync(createdSale.Id, ct)
                ?? throw new CustomBusinessException("Ops... O registro buscado não foi encontrado.");

            return _mapper.Map<SaleResponseDTO>(confirmedSale);
        }

        private async Task<ProposalItemEntity> BuildProposalItemAsync(
            CreateProposalItemRequestDTO itemRequest,
            DateTime now,
            string userEmail,
            CancellationToken ct)
        {
            return await BuildProposalItemCoreAsync(
                itemRequest.ProductId,
                itemRequest.Quantity,
                itemRequest.UnitPrice,
                itemRequest.DiscountPercent,
                itemRequest.DiscountAmount,
                itemRequest.IcmsPercent,
                itemRequest.IssPercent,
                itemRequest.PisPercent,
                itemRequest.CofinsPercent,
                now,
                userEmail,
                ct);
        }

        private async Task<ProposalItemEntity> BuildProposalItemAsync(
            UpdateProposalItemRequestDTO itemRequest,
            DateTime now,
            string userEmail,
            CancellationToken ct)
        {
            return await BuildProposalItemCoreAsync(
                itemRequest.ProductId,
                itemRequest.Quantity,
                itemRequest.UnitPrice,
                itemRequest.DiscountPercent,
                itemRequest.DiscountAmount,
                itemRequest.IcmsPercent,
                itemRequest.IssPercent,
                itemRequest.PisPercent,
                itemRequest.CofinsPercent,
                now,
                userEmail,
                ct);
        }

        private async Task<ProposalItemEntity> BuildProposalItemCoreAsync(
            int productId,
            decimal quantity,
            decimal unitPrice,
            decimal discountPercent,
            decimal discountAmount,
            decimal? icmsPercent,
            decimal? issPercent,
            decimal? pisPercent,
            decimal? cofinsPercent,
            DateTime now,
            string userEmail,
            CancellationToken ct)
        {
            ProductEntity product = await _productRepository.GetByIdAsync(productId)
                ?? throw new CustomBusinessException("Ops... O registro buscado não foi encontrado.");

            decimal effectiveUnitPrice = unitPrice > 0 ? unitPrice : product.UnitPrice;
            decimal effectiveIcmsPercent = icmsPercent ?? product.IcmsPercent;
            decimal effectiveIssPercent = issPercent ?? product.IssPercent;
            decimal effectivePisPercent = pisPercent ?? product.PisPercent;
            decimal effectiveCofinsPercent = cofinsPercent ?? product.CofinsPercent;

            decimal lineSubtotal = SalesCalculationHelper.CalculateLineSubtotal(
                quantity, effectiveUnitPrice, discountPercent, discountAmount);

            decimal icmsAmount = SalesCalculationHelper.CalculateTaxAmount(lineSubtotal, effectiveIcmsPercent);
            decimal issAmount = SalesCalculationHelper.CalculateTaxAmount(lineSubtotal, effectiveIssPercent);
            decimal pisAmount = SalesCalculationHelper.CalculateTaxAmount(lineSubtotal, effectivePisPercent);
            decimal cofinsAmount = SalesCalculationHelper.CalculateTaxAmount(lineSubtotal, effectiveCofinsPercent);

            return new ProposalItemEntity
            {
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = effectiveUnitPrice,
                DiscountPercent = discountPercent,
                DiscountAmount = discountAmount,
                LineSubtotal = lineSubtotal,
                IcmsPercent = effectiveIcmsPercent,
                IcmsAmount = icmsAmount,
                IssPercent = effectiveIssPercent,
                IssAmount = issAmount,
                PisPercent = effectivePisPercent,
                PisAmount = pisAmount,
                CofinsPercent = effectiveCofinsPercent,
                CofinsAmount = cofinsAmount,
                LineTotal = SalesCalculationHelper.CalculateLineTotal(lineSubtotal, icmsAmount, issAmount, pisAmount, cofinsAmount),
                CreatedAt = now,
                CreatedBy = userEmail
            };
        }

        private async Task ApplyStockWarningsAsync(ProposalEntity entity, CancellationToken ct)
        {
            bool hasStockWarning = false;

            foreach (ProposalItemEntity item in entity.Items)
            {
                ProductStockEntity? stock = await _productStockRepository.GetByProductIdAsync(item.ProductId, ct);
                decimal physical = stock?.Quantity ?? 0m;
                decimal reserved = await _repository.GetReservedQuantityByProductIdAsync(item.ProductId, ct);
                decimal available = physical - reserved;

                item.StockUnavailable = available < item.Quantity;
                if (item.StockUnavailable)
                    hasStockWarning = true;
            }

            entity.HasStockWarning = hasStockWarning;
        }

        private static void RecalculateProposalTotals(ProposalEntity entity)
        {
            entity.SubtotalAmount = entity.Items.Sum(i => i.LineSubtotal);
            entity.DiscountAmount = entity.Items.Sum(i => i.DiscountAmount);
            entity.TaxAmount = entity.Items.Sum(i => i.IcmsAmount + i.IssAmount + i.PisAmount + i.CofinsAmount);
            entity.TotalAmount = entity.Items.Sum(i => i.LineTotal);
        }

        private async Task<ProposalEntity> GetProposalWithItemsAsync(int id, CancellationToken ct)
        {
            return await _repository.GetByIdWithItemsAsync(id, ct)
                ?? throw new CustomBusinessException("Ops... O registro buscado não foi encontrado.");
        }

        private static void EnsureEditable(ProposalEntity entity)
        {
            if (entity.Status != ProposalStatusEnum.Draft)
                throw new CustomBusinessException("Ops... Somente propostas em rascunho podem ser alteradas.");
        }

        private DateTime GetCurrentDateTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone);
        }

        private string GetCurrentUserEmail()
        {
            return _email;
        }
    }
}
