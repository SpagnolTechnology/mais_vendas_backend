using Crosscutting.DTO.Test;

namespace AppService.AppService.Interfaces
{
    public interface ITestAppService
    {
        Task<IReadOnlyList<TestResponseDTO>> GetAllAsync(CancellationToken ct = default);
        Task<TestResponseDTO> GetResponseByIdAsync(int id);
        Task<TestResponseDTO> CreateAsync(CreateTestRequestDTO request, CancellationToken ct = default);
        Task<TestResponseDTO> UpdateAsync(int id, UpdateTestRequestDTO request);
        Task DeleteAsync(int id);
    }
}
