using AppService.AppService.Interfaces;
using AutoMapper;
using Crosscutting.CustomException;
using Crosscutting.DTO.CommissionRule;
using Domain.Entity;
using Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AppService.AppService.Services
{
    public class CommissionRuleAppService : BaseService<ICommissionRuleRepository, CommissionRuleEntity>, ICommissionRuleAppService
    {
        public CommissionRuleAppService(
            IMapper mapper,
            ICommissionRuleRepository repository,
            IHttpContextAccessor httpContextAccessor) : base(mapper, repository, httpContextAccessor)
        {
        }

        public async Task<IReadOnlyList<CommissionRuleResponseDTO>> GetAllAsync(CancellationToken ct = default)
        {
            IReadOnlyList<CommissionRuleEntity> entities = await GetAllReadOnlyAsync(ct);
            return _mapper.Map<IReadOnlyList<CommissionRuleResponseDTO>>(entities);
        }

        public async Task<CommissionRuleResponseDTO> GetResponseByIdAsync(int id)
        {
            CommissionRuleEntity entity = await base.GetByIdAsync(id);
            return _mapper.Map<CommissionRuleResponseDTO>(entity);
        }

        public async Task<CommissionRuleResponseDTO> CreateAsync(CreateCommissionRuleRequestDTO request, CancellationToken ct = default)
        {
            ValidateGlobalRule(request.ProductId, request.CalculationScope);

            CommissionRuleEntity entity = _mapper.Map<CommissionRuleEntity>(request);
            entity.CreatedAt = GetCurrentDateTime();
            entity.CreatedBy = GetCurrentUserEmail();

            CommissionRuleEntity createdEntity = await AddAsync(entity, ct);
            return _mapper.Map<CommissionRuleResponseDTO>(createdEntity);
        }

        public async Task<CommissionRuleResponseDTO> UpdateAsync(int id, UpdateCommissionRuleRequestDTO request)
        {
            ValidateGlobalRule(request.ProductId, request.CalculationScope);

            CommissionRuleEntity entity = await base.GetByIdAsync(id);

            entity.ProductId = request.ProductId;
            entity.CalculationScope = request.CalculationScope;
            entity.CommissionPercent = request.CommissionPercent;
            entity.IsActive = request.IsActive;
            entity.UpdatedAt = GetCurrentDateTime();
            entity.UpdatedBy = GetCurrentUserEmail();

            CommissionRuleEntity updatedEntity = await EditAsync(entity);
            return _mapper.Map<CommissionRuleResponseDTO>(updatedEntity);
        }

        public async Task DeleteAsync(int id)
        {
            CommissionRuleEntity entity = await base.GetByIdAsync(id);
            await DeleteAsync(entity);
        }

        private static void ValidateGlobalRule(int? productId, Crosscutting.Enum.CommissionCalculationScopeEnum? calculationScope)
        {
            if (productId == null && calculationScope == null)
                throw new CustomBusinessException("Ops... A regra global de comissão deve definir o escopo de cálculo.");
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
