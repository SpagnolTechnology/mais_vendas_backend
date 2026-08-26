using Crosscutting.DTO.UnitOfMeasure;

namespace AppService.AppService.Interfaces
{
    public interface IUnitOfMeasureAppService
    {
        Task<IReadOnlyList<UnitOfMeasureResponseDTO>> GetAllAsync(CancellationToken ct = default);
        Task<UnitOfMeasureResponseDTO> GetResponseByIdAsync(int id);
        Task<UnitOfMeasureResponseDTO> CreateAsync(CreateUnitOfMeasureRequestDTO request, CancellationToken ct = default);
        Task<UnitOfMeasureResponseDTO> UpdateAsync(int id, UpdateUnitOfMeasureRequestDTO request);
        Task DeleteAsync(int id);
    }
}
