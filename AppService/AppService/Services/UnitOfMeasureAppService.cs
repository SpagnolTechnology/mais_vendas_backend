using AppService.AppService.Interfaces;
using AutoMapper;
using Crosscutting.DTO.UnitOfMeasure;
using Domain.Entity;
using Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AppService.AppService.Services
{
    public class UnitOfMeasureAppService : BaseService<IUnitOfMeasureRepository, UnitOfMeasureEntity>, IUnitOfMeasureAppService
    {
        public UnitOfMeasureAppService(
            IMapper mapper,
            IUnitOfMeasureRepository repository,
            IHttpContextAccessor httpContextAccessor) : base(mapper, repository, httpContextAccessor)
        {
        }

        public async Task<IReadOnlyList<UnitOfMeasureResponseDTO>> GetAllAsync(CancellationToken ct = default)
        {
            IReadOnlyList<UnitOfMeasureEntity> entities = await GetAllReadOnlyAsync(ct);
            return _mapper.Map<IReadOnlyList<UnitOfMeasureResponseDTO>>(entities);
        }

        public async Task<UnitOfMeasureResponseDTO> GetResponseByIdAsync(int id)
        {
            UnitOfMeasureEntity entity = await base.GetByIdAsync(id);
            return _mapper.Map<UnitOfMeasureResponseDTO>(entity);
        }

        public async Task<UnitOfMeasureResponseDTO> CreateAsync(CreateUnitOfMeasureRequestDTO request, CancellationToken ct = default)
        {
            UnitOfMeasureEntity entity = _mapper.Map<UnitOfMeasureEntity>(request);
            entity.CreatedAt = GetCurrentDateTime();
            entity.CreatedBy = GetCurrentUserEmail();

            UnitOfMeasureEntity createdEntity = await AddAsync(entity, ct);
            return _mapper.Map<UnitOfMeasureResponseDTO>(createdEntity);
        }

        public async Task<UnitOfMeasureResponseDTO> UpdateAsync(int id, UpdateUnitOfMeasureRequestDTO request)
        {
            UnitOfMeasureEntity entity = await base.GetByIdAsync(id);

            entity.Name = request.Name;
            entity.Abbreviation = request.Abbreviation;
            entity.IsActive = request.IsActive;
            entity.UpdatedAt = GetCurrentDateTime();
            entity.UpdatedBy = GetCurrentUserEmail();

            UnitOfMeasureEntity updatedEntity = await EditAsync(entity);
            return _mapper.Map<UnitOfMeasureResponseDTO>(updatedEntity);
        }

        public async Task DeleteAsync(int id)
        {
            UnitOfMeasureEntity entity = await base.GetByIdAsync(id);
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
