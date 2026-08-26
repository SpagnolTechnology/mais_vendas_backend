using Crosscutting.DTO.StockAdjustment;

namespace AppService.AppService.Interfaces
{
    public interface IStockAdjustmentAppService
    {
        Task<IReadOnlyList<StockAdjustmentResponseDTO>> GetAllAsync(CancellationToken ct = default);
        Task<StockAdjustmentResponseDTO> GetResponseByIdAsync(int id, CancellationToken ct = default);
        Task<StockAdjustmentResponseDTO> CreateAsync(CreateStockAdjustmentRequestDTO request, CancellationToken ct = default);
        Task<StockAdjustmentResponseDTO> UpdateAsync(int id, UpdateStockAdjustmentRequestDTO request, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
        Task ConfirmAsync(int id, CancellationToken ct = default);
    }
}
