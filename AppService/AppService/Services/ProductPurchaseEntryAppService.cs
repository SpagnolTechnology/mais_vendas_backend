using AppService.AppService.Interfaces;
using AutoMapper;
using Crosscutting.CustomException;
using Crosscutting.DTO.ProductPurchaseEntry;
using Crosscutting.Enum;
using Crosscutting.Helpers;
using Domain.Entity;
using Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AppService.AppService.Services
{
    public class ProductPurchaseEntryAppService : BaseService<IProductPurchaseEntryRepository, ProductPurchaseEntryEntity>, IProductPurchaseEntryAppService
    {
        private readonly ISalesOperationsRepository _salesOperationsRepository;

        public ProductPurchaseEntryAppService(
            IMapper mapper,
            IProductPurchaseEntryRepository repository,
            ISalesOperationsRepository salesOperationsRepository,
            IHttpContextAccessor httpContextAccessor) : base(mapper, repository, httpContextAccessor)
        {
            _salesOperationsRepository = salesOperationsRepository;
        }

        public async Task<IReadOnlyList<ProductPurchaseEntryResponseDTO>> GetAllAsync(CancellationToken ct = default)
        {
            IReadOnlyList<ProductPurchaseEntryEntity> entities = await GetAllReadOnlyAsync(ct);
            return _mapper.Map<IReadOnlyList<ProductPurchaseEntryResponseDTO>>(entities);
        }

        public async Task<ProductPurchaseEntryResponseDTO> GetResponseByIdAsync(int id, CancellationToken ct = default)
        {
            ProductPurchaseEntryEntity entity = await GetEntryWithItemsAsync(id, ct);
            return _mapper.Map<ProductPurchaseEntryResponseDTO>(entity);
        }

        public async Task<ProductPurchaseEntryResponseDTO> CreateAsync(CreateProductPurchaseEntryRequestDTO request, CancellationToken ct = default)
        {
            if (!request.Items.Any())
                throw new CustomBusinessException("Ops... A entrada de compra deve conter ao menos um item.");

            DateTime now = GetCurrentDateTime();
            string userEmail = GetCurrentUserEmail();

            ProductPurchaseEntryEntity entity = _mapper.Map<ProductPurchaseEntryEntity>(request);
            entity.Status = PurchaseEntryStatusEnum.Draft;
            entity.CreatedAt = now;
            entity.CreatedBy = userEmail;

            foreach (CreateProductPurchaseEntryItemRequestDTO itemRequest in request.Items)
            {
                ProductPurchaseEntryItemEntity item = _mapper.Map<ProductPurchaseEntryItemEntity>(itemRequest);
                item.CalculatedUnitPrice = SalesCalculationHelper.CalculateUnitPriceFromCost(item.UnitCost, item.MarkupPercent);
                item.TotalCost = item.UnitCost * item.Quantity;
                item.CreatedAt = now;
                item.CreatedBy = userEmail;
                entity.Items.Add(item);
            }

            ProductPurchaseEntryEntity createdEntity = await AddAsync(entity, ct);
            ProductPurchaseEntryEntity entityWithItems = await GetEntryWithItemsAsync(createdEntity.Id, ct);
            return _mapper.Map<ProductPurchaseEntryResponseDTO>(entityWithItems);
        }

        public async Task<ProductPurchaseEntryResponseDTO> UpdateAsync(int id, UpdateProductPurchaseEntryRequestDTO request, CancellationToken ct = default)
        {
            ProductPurchaseEntryEntity entity = await GetEntryWithItemsAsync(id, ct);
            EnsureDraft(entity);

            if (!request.Items.Any())
                throw new CustomBusinessException("Ops... A entrada de compra deve conter ao menos um item.");

            DateTime now = GetCurrentDateTime();
            string userEmail = GetCurrentUserEmail();

            entity.SupplierId = request.SupplierId;
            entity.InvoiceNumber = request.InvoiceNumber;
            entity.InvoiceSeries = request.InvoiceSeries;
            entity.InvoiceKey = request.InvoiceKey;
            entity.EntryDate = request.EntryDate;
            entity.Notes = request.Notes;
            entity.UpdatedAt = now;
            entity.UpdatedBy = userEmail;

            entity.Items.Clear();

            foreach (UpdateProductPurchaseEntryItemRequestDTO itemRequest in request.Items)
            {
                ProductPurchaseEntryItemEntity item = new()
                {
                    ProductId = itemRequest.ProductId,
                    Quantity = itemRequest.Quantity,
                    UnitCost = itemRequest.UnitCost,
                    MarkupPercent = itemRequest.MarkupPercent,
                    CalculatedUnitPrice = SalesCalculationHelper.CalculateUnitPriceFromCost(itemRequest.UnitCost, itemRequest.MarkupPercent),
                    TotalCost = itemRequest.UnitCost * itemRequest.Quantity,
                    CreatedAt = now,
                    CreatedBy = userEmail
                };
                entity.Items.Add(item);
            }

            ProductPurchaseEntryEntity updatedEntity = await EditAsync(entity);
            ProductPurchaseEntryEntity entityWithItems = await GetEntryWithItemsAsync(updatedEntity.Id, ct);
            return _mapper.Map<ProductPurchaseEntryResponseDTO>(entityWithItems);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            ProductPurchaseEntryEntity entity = await GetEntryWithItemsAsync(id, ct);
            EnsureDraft(entity);
            await DeleteAsync(entity);
        }

        public async Task ConfirmAsync(int id, CancellationToken ct = default)
        {
            ProductPurchaseEntryEntity entity = await GetEntryWithItemsAsync(id, ct);
            EnsureDraft(entity);

            DateTime now = GetCurrentDateTime();
            await _salesOperationsRepository.ConfirmPurchaseEntryAsync(id, GetCurrentUserEmail(), now, ct);
        }

        private async Task<ProductPurchaseEntryEntity> GetEntryWithItemsAsync(int id, CancellationToken ct)
        {
            return await _repository.GetByIdWithItemsAsync(id, ct)
                ?? throw new CustomBusinessException("Ops... O registro buscado não foi encontrado.");
        }

        private static void EnsureDraft(ProductPurchaseEntryEntity entity)
        {
            if (entity.Status == PurchaseEntryStatusEnum.Confirmed)
                throw new CustomBusinessException("Ops... Não é possível alterar uma entrada de compra já confirmada.");
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
