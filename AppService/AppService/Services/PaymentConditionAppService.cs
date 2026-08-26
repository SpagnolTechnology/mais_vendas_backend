using AppService.AppService.Interfaces;
using AutoMapper;
using Crosscutting.DTO.PaymentCondition;
using Domain.Entity;
using Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AppService.AppService.Services
{
    public class PaymentConditionAppService : BaseService<IPaymentConditionRepository, PaymentConditionEntity>, IPaymentConditionAppService
    {
        public PaymentConditionAppService(
            IMapper mapper,
            IPaymentConditionRepository repository,
            IHttpContextAccessor httpContextAccessor) : base(mapper, repository, httpContextAccessor)
        {
        }

        public async Task<IReadOnlyList<PaymentConditionResponseDTO>> GetAllAsync(CancellationToken ct = default)
        {
            IReadOnlyList<PaymentConditionEntity> entities = await GetAllReadOnlyAsync(ct);
            return _mapper.Map<IReadOnlyList<PaymentConditionResponseDTO>>(entities);
        }

        public async Task<PaymentConditionResponseDTO> GetResponseByIdAsync(int id)
        {
            PaymentConditionEntity entity = await base.GetByIdAsync(id);
            return _mapper.Map<PaymentConditionResponseDTO>(entity);
        }

        public async Task<PaymentConditionResponseDTO> CreateAsync(CreatePaymentConditionRequestDTO request, CancellationToken ct = default)
        {
            PaymentConditionEntity entity = _mapper.Map<PaymentConditionEntity>(request);
            entity.CreatedAt = GetCurrentDateTime();
            entity.CreatedBy = GetCurrentUserEmail();

            PaymentConditionEntity createdEntity = await AddAsync(entity, ct);
            return _mapper.Map<PaymentConditionResponseDTO>(createdEntity);
        }

        public async Task<PaymentConditionResponseDTO> UpdateAsync(int id, UpdatePaymentConditionRequestDTO request)
        {
            PaymentConditionEntity entity = await base.GetByIdAsync(id);

            entity.Name = request.Name;
            entity.InstallmentCount = request.InstallmentCount;
            entity.DaysUntilFirstDue = request.DaysUntilFirstDue;
            entity.DaysBetweenInstallments = request.DaysBetweenInstallments;
            entity.CashDiscountPercent = request.CashDiscountPercent;
            entity.IsActive = request.IsActive;
            entity.UpdatedAt = GetCurrentDateTime();
            entity.UpdatedBy = GetCurrentUserEmail();

            PaymentConditionEntity updatedEntity = await EditAsync(entity);
            return _mapper.Map<PaymentConditionResponseDTO>(updatedEntity);
        }

        public async Task DeleteAsync(int id)
        {
            PaymentConditionEntity entity = await base.GetByIdAsync(id);
            await DeleteAsync(entity);
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
