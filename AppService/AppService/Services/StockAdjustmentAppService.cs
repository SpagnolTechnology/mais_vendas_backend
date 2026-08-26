using AppService.AppService.Interfaces;
using AutoMapper;
using Crosscutting.CustomException;
using Crosscutting.DTO.StockAdjustment;
using Domain.Entity;
using Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AppService.AppService.Services
{
    public class StockAdjustmentAppService : BaseService<IStockAdjustmentRepository, StockAdjustmentEntity>, IStockAdjustmentAppService
    {
        private readonly ISalesOperationsRepository _salesOperationsRepository;

        public StockAdjustmentAppService(
            IMapper mapper,
            IStockAdjustmentRepository repository,
            ISalesOperationsRepository salesOperationsRepository,
            IHttpContextAccessor httpContextAccessor) : base(mapper, repository, httpContextAccessor)
        {
            _salesOperationsRepository = salesOperationsRepository;
        }

        public async Task<IReadOnlyList<StockAdjustmentResponseDTO>> GetAllAsync(CancellationToken ct = default)
        {
            IReadOnlyList<StockAdjustmentEntity> entities = await GetAllReadOnlyAsync(ct);
            return _mapper.Map<IReadOnlyList<StockAdjustmentResponseDTO>>(entities);
        }

        public async Task<StockAdjustmentResponseDTO> GetResponseByIdAsync(int id, CancellationToken ct = default)
        {
            StockAdjustmentEntity entity = await GetAdjustmentWithItemsAsync(id, ct);
            return _mapper.Map<StockAdjustmentResponseDTO>(entity);
        }

        public async Task<StockAdjustmentResponseDTO> CreateAsync(CreateStockAdjustmentRequestDTO request, CancellationToken ct = default)
        {
            if (!request.Items.Any())
                throw new CustomBusinessException("Ops... O acerto de estoque deve conter ao menos um item.");

            DateTime now = GetCurrentDateTime();
            string userEmail = GetCurrentUserEmail();

            StockAdjustmentEntity entity = _mapper.Map<StockAdjustmentEntity>(request);
            entity.IsConfirmed = false;
            entity.CreatedAt = now;
            entity.CreatedBy = userEmail;

            foreach (CreateStockAdjustmentItemRequestDTO itemRequest in request.Items)
            {
                StockAdjustmentItemEntity item = _mapper.Map<StockAdjustmentItemEntity>(itemRequest);
                item.CreatedAt = now;
                item.CreatedBy = userEmail;
                entity.Items.Add(item);
            }

            StockAdjustmentEntity createdEntity = await AddAsync(entity, ct);
            StockAdjustmentEntity entityWithItems = await GetAdjustmentWithItemsAsync(createdEntity.Id, ct);
            return _mapper.Map<StockAdjustmentResponseDTO>(entityWithItems);
        }

        public async Task<StockAdjustmentResponseDTO> UpdateAsync(int id, UpdateStockAdjustmentRequestDTO request, CancellationToken ct = default)
        {
            StockAdjustmentEntity entity = await GetAdjustmentWithItemsAsync(id, ct);
            EnsureDraft(entity);

            if (!request.Items.Any())
                throw new CustomBusinessException("Ops... O acerto de estoque deve conter ao menos um item.");

            DateTime now = GetCurrentDateTime();
            string userEmail = GetCurrentUserEmail();

            entity.AdjustmentDate = request.AdjustmentDate;
            entity.Reason = request.Reason;
            entity.Notes = request.Notes;
            entity.UpdatedAt = now;
            entity.UpdatedBy = userEmail;

            entity.Items.Clear();

            foreach (UpdateStockAdjustmentItemRequestDTO itemRequest in request.Items)
            {
                StockAdjustmentItemEntity item = new()
                {
                    ProductId = itemRequest.ProductId,
                    Quantity = itemRequest.Quantity,
                    Notes = itemRequest.Notes,
                    CreatedAt = now,
                    CreatedBy = userEmail
                };
                entity.Items.Add(item);
            }

            StockAdjustmentEntity updatedEntity = await EditAsync(entity);
            StockAdjustmentEntity entityWithItems = await GetAdjustmentWithItemsAsync(updatedEntity.Id, ct);
            return _mapper.Map<StockAdjustmentResponseDTO>(entityWithItems);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            StockAdjustmentEntity entity = await GetAdjustmentWithItemsAsync(id, ct);
            EnsureDraft(entity);
            await DeleteAsync(entity);
        }

        public async Task ConfirmAsync(int id, CancellationToken ct = default)
        {
            StockAdjustmentEntity entity = await GetAdjustmentWithItemsAsync(id, ct);
            EnsureDraft(entity);

            DateTime now = GetCurrentDateTime();
            await _salesOperationsRepository.ConfirmStockAdjustmentAsync(id, GetCurrentUserEmail(), now, ct);
        }

        private async Task<StockAdjustmentEntity> GetAdjustmentWithItemsAsync(int id, CancellationToken ct)
        {
            return await _repository.GetByIdWithItemsAsync(id, ct)
                ?? throw new CustomBusinessException("Ops... O registro buscado não foi encontrado.");
        }

        private static void EnsureDraft(StockAdjustmentEntity entity)
        {
            if (entity.IsConfirmed)
                throw new CustomBusinessException("Ops... Não é possível alterar um acerto de estoque já confirmado.");
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
