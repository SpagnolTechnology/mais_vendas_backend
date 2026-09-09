using Crosscutting.DTO.Product;
using Crosscutting.DTO.StockMovement;

namespace AppService.AppService.Interfaces
{
    public interface IProductAppService
    {
        Task<IReadOnlyList<ProductResponseDTO>> GetAllAsync(CancellationToken ct = default);
        Task<ProductResponseDTO> GetResponseByIdAsync(int id, CancellationToken ct = default);
        Task<ProductStockSummaryResponseDTO> GetStockSummaryAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<StockMovementResponseDTO>> GetMovementsAsync(int id, CancellationToken ct = default);
        Task<ProductResponseDTO> CreateAsync(CreateProductRequestDTO request, CancellationToken ct = default);
        Task<ProductResponseDTO> UpdateAsync(int id, UpdateProductRequestDTO request);
        Task DeleteAsync(int id);
        Task<IReadOnlyList<ProductPhotoResponseDTO>> GetPhotosAsync(int productId, CancellationToken ct = default);
        Task<ProductPhotoResponseDTO> CreatePhotoAsync(int productId, CreateProductPhotoRequestDTO request, CancellationToken ct = default);
        Task<ProductPhotoResponseDTO> UpdatePhotoAsync(int productId, int photoId, UpdateProductPhotoRequestDTO request);
        Task DeletePhotoAsync(int productId, int photoId);
    }
}
