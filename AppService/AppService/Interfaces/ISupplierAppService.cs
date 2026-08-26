using Crosscutting.DTO.Supplier;

namespace AppService.AppService.Interfaces
{
    public interface ISupplierAppService
    {
        Task<IReadOnlyList<SupplierResponseDTO>> GetAllAsync(CancellationToken ct = default);
        Task<SupplierResponseDTO> GetResponseByIdAsync(int id);
        Task<SupplierResponseDTO> CreateAsync(CreateSupplierRequestDTO request, CancellationToken ct = default);
        Task<SupplierResponseDTO> UpdateAsync(int id, UpdateSupplierRequestDTO request);
        Task DeleteAsync(int id);
    }
}
