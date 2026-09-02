using Crosscutting.DTO.ProductPurchaseEntry;

namespace AppService.AppService.Interfaces
{
    public interface IProductPurchaseEntryAppService
    {
        Task<IReadOnlyList<ProductPurchaseEntryResponseDTO>> GetAllAsync(CancellationToken ct = default);
        Task<ProductPurchaseEntryResponseDTO> GetResponseByIdAsync(int id, CancellationToken ct = default);
        Task<ProductPurchaseEntryResponseDTO> CreateAsync(CreateProductPurchaseEntryRequestDTO request, CancellationToken ct = default);
        Task<ProductPurchaseEntryResponseDTO> CreateAndConfirmAsync(CreateProductPurchaseEntryRequestDTO request, CancellationToken ct = default);
        Task<ProductPurchaseEntryResponseDTO> UpdateAsync(int id, UpdateProductPurchaseEntryRequestDTO request, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
        Task ConfirmAsync(int id, CancellationToken ct = default);
    }
}
