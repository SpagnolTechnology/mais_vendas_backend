using Crosscutting.DTO.Commission;

namespace AppService.AppService.Interfaces
{
    public interface ICommissionAppService
    {
        Task<IReadOnlyList<CommissionResponseDTO>> GetBySaleIdAsync(int saleId, CancellationToken ct = default);
        Task<CommissionResponseDTO> UpdateStatusAsync(int id, UpdateCommissionStatusRequestDTO request);
    }
}
