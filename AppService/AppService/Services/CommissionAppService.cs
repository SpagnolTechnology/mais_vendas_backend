using AppService.AppService.Interfaces;
using AutoMapper;
using Crosscutting.DTO.Commission;
using Domain.Entity;
using Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AppService.AppService.Services
{
    public class CommissionAppService : BaseService<ICommissionRepository, CommissionEntity>, ICommissionAppService
    {
        public CommissionAppService(
            IMapper mapper,
            ICommissionRepository repository,
            IHttpContextAccessor httpContextAccessor) : base(mapper, repository, httpContextAccessor)
        {
        }

        public async Task<IReadOnlyList<CommissionResponseDTO>> GetBySaleIdAsync(int saleId, CancellationToken ct = default)
        {
            IReadOnlyList<CommissionEntity> entities = await GetAllReadOnlyAsync(ct);
            IEnumerable<CommissionEntity> filtered = entities.Where(c => c.SaleId == saleId);
            return _mapper.Map<IReadOnlyList<CommissionResponseDTO>>(filtered.ToList());
        }

        public async Task<CommissionResponseDTO> UpdateStatusAsync(int id, UpdateCommissionStatusRequestDTO request)
        {
            CommissionEntity entity = await base.GetByIdAsync(id);

            entity.Status = request.Status;
            entity.UpdatedAt = GetCurrentDateTime();
            entity.UpdatedBy = GetCurrentUserEmail();

            CommissionEntity updatedEntity = await EditAsync(entity);
            return _mapper.Map<CommissionResponseDTO>(updatedEntity);
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
