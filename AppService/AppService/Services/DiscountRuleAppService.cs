using AppService.AppService.Interfaces;
using AutoMapper;
using Crosscutting.DTO.DiscountRule;
using Domain.Entity;
using Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AppService.AppService.Services
{
    public class DiscountRuleAppService : BaseService<IDiscountRuleRepository, DiscountRuleEntity>, IDiscountRuleAppService
    {
        public DiscountRuleAppService(
            IMapper mapper,
            IDiscountRuleRepository repository,
            IHttpContextAccessor httpContextAccessor) : base(mapper, repository, httpContextAccessor)
        {
        }

        public async Task<IReadOnlyList<DiscountRuleResponseDTO>> GetAllAsync(CancellationToken ct = default)
        {
            IReadOnlyList<DiscountRuleEntity> entities = await GetAllReadOnlyAsync(ct);
            return _mapper.Map<IReadOnlyList<DiscountRuleResponseDTO>>(entities);
        }

        public async Task<DiscountRuleResponseDTO> GetResponseByIdAsync(int id)
        {
            DiscountRuleEntity entity = await base.GetByIdAsync(id);
            return _mapper.Map<DiscountRuleResponseDTO>(entity);
        }

        public async Task<DiscountRuleResponseDTO> CreateAsync(CreateDiscountRuleRequestDTO request, CancellationToken ct = default)
        {
            DiscountRuleEntity entity = _mapper.Map<DiscountRuleEntity>(request);
            entity.CreatedAt = GetCurrentDateTime();
            entity.CreatedBy = GetCurrentUserEmail();

            DiscountRuleEntity createdEntity = await AddAsync(entity, ct);
            return _mapper.Map<DiscountRuleResponseDTO>(createdEntity);
        }

        public async Task<DiscountRuleResponseDTO> UpdateAsync(int id, UpdateDiscountRuleRequestDTO request)
        {
            DiscountRuleEntity entity = await base.GetByIdAsync(id);

            entity.Role = request.Role;
            entity.UserEmail = request.UserEmail;
            entity.MaxDiscountPercent = request.MaxDiscountPercent;
            entity.MaxDiscountAmount = request.MaxDiscountAmount;
            entity.IsActive = request.IsActive;
            entity.UpdatedAt = GetCurrentDateTime();
            entity.UpdatedBy = GetCurrentUserEmail();

            DiscountRuleEntity updatedEntity = await EditAsync(entity);
            return _mapper.Map<DiscountRuleResponseDTO>(updatedEntity);
        }

        public async Task DeleteAsync(int id)
        {
            DiscountRuleEntity entity = await base.GetByIdAsync(id);
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
