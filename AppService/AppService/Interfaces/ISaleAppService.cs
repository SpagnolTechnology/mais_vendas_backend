using Crosscutting.DTO.Sale;

namespace AppService.AppService.Interfaces
{
    public interface ISaleAppService
    {
        Task<SaleResponseDTO> GetResponseByIdAsync(int id, CancellationToken ct = default);
        Task<SaleResponseDTO> CreateAsync(CreateSaleRequestDTO request, CancellationToken ct = default);
        Task<SaleResponseDTO> ConfirmAsync(int id, CancellationToken ct = default);
    }
}
