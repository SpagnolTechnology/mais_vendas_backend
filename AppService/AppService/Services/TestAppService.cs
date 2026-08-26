using AppService.AppService.Interfaces;
using AutoMapper;
using Crosscutting.DTO.Test;
using Domain.Entity;
using Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AppService.AppService.Services
{
    public class TestAppService : BaseService<ITestRepository, TestEntity>, ITestAppService
    {
        public TestAppService(
            IMapper mapper,
            ITestRepository repository,
            IHttpContextAccessor httpContextAccessor) : base(mapper, repository, httpContextAccessor)
        {
        }

        public async Task<IReadOnlyList<TestResponseDTO>> GetAllAsync(CancellationToken ct = default)
        {
            IReadOnlyList<TestEntity> entities = await GetAllReadOnlyAsync(ct);
            return _mapper.Map<IReadOnlyList<TestResponseDTO>>(entities);
        }

        public async Task<TestResponseDTO> GetResponseByIdAsync(int id)
        {
            TestEntity entity = await base.GetByIdAsync(id);
            return _mapper.Map<TestResponseDTO>(entity);
        }

        public async Task<TestResponseDTO> CreateAsync(CreateTestRequestDTO request, CancellationToken ct = default)
        {
            TestEntity entity = _mapper.Map<TestEntity>(request);
            entity.CreatedAt = GetCurrentDateTime();
            entity.CreatedBy = GetCurrentUserEmail();

            TestEntity createdEntity = await AddAsync(entity, ct);
            return _mapper.Map<TestResponseDTO>(createdEntity);
        }

        public async Task<TestResponseDTO> UpdateAsync(int id, UpdateTestRequestDTO request)
        {
            TestEntity entity = await base.GetByIdAsync(id);

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.IsActive = request.IsActive;
            entity.ReferenceDate = request.ReferenceDate;
            entity.UpdatedAt = GetCurrentDateTime();
            entity.UpdatedBy = GetCurrentUserEmail();

            TestEntity updatedEntity = await EditAsync(entity);
            return _mapper.Map<TestResponseDTO>(updatedEntity);
        }

        public async Task DeleteAsync(int id)
        {
            TestEntity entity = await base.GetByIdAsync(id);
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
