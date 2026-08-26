using Crosscutting.DTO.Discount;

namespace AppService.AppService.Interfaces
{
    public interface IDiscountAppService
    {
        Task<DiscountResponseDTO> ApplyDiscountAsync(ApplyDiscountRequestDTO request, CancellationToken ct = default);
        Task<IReadOnlyList<DiscountResponseDTO>> GetByProposalIdAsync(int proposalId, CancellationToken ct = default);
        Task<IReadOnlyList<DiscountResponseDTO>> GetBySaleIdAsync(int saleId, CancellationToken ct = default);
    }
}
