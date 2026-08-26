using AppService.AppService.Interfaces;
using AutoMapper;
using Crosscutting.DTO.Supplier;
using Domain.Entity;
using Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AppService.AppService.Services
{
    public class SupplierAppService : BaseService<ISupplierRepository, SupplierEntity>, ISupplierAppService
    {
        public SupplierAppService(
            IMapper mapper,
            ISupplierRepository repository,
            IHttpContextAccessor httpContextAccessor) : base(mapper, repository, httpContextAccessor)
        {
        }

        public async Task<IReadOnlyList<SupplierResponseDTO>> GetAllAsync(CancellationToken ct = default)
        {
            IReadOnlyList<SupplierEntity> entities = await GetAllReadOnlyAsync(ct);
            return _mapper.Map<IReadOnlyList<SupplierResponseDTO>>(entities);
        }

        public async Task<SupplierResponseDTO> GetResponseByIdAsync(int id)
        {
            SupplierEntity entity = await base.GetByIdAsync(id);
            return _mapper.Map<SupplierResponseDTO>(entity);
        }

        public async Task<SupplierResponseDTO> CreateAsync(CreateSupplierRequestDTO request, CancellationToken ct = default)
        {
            SupplierEntity entity = _mapper.Map<SupplierEntity>(request);
            entity.CreatedAt = GetCurrentDateTime();
            entity.CreatedBy = GetCurrentUserEmail();

            SupplierEntity createdEntity = await AddAsync(entity, ct);
            return _mapper.Map<SupplierResponseDTO>(createdEntity);
        }

        public async Task<SupplierResponseDTO> UpdateAsync(int id, UpdateSupplierRequestDTO request)
        {
            SupplierEntity entity = await base.GetByIdAsync(id);

            entity.Name = request.Name;
            entity.Document = request.Document;
            entity.Email = request.Email;
            entity.Phone = request.Phone;
            entity.Address = request.Address;
            entity.City = request.City;
            entity.State = request.State;
            entity.ZipCode = request.ZipCode;
            entity.IsActive = request.IsActive;
            entity.UpdatedAt = GetCurrentDateTime();
            entity.UpdatedBy = GetCurrentUserEmail();

            SupplierEntity updatedEntity = await EditAsync(entity);
            return _mapper.Map<SupplierResponseDTO>(updatedEntity);
        }

        public async Task DeleteAsync(int id)
        {
            SupplierEntity entity = await base.GetByIdAsync(id);
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
