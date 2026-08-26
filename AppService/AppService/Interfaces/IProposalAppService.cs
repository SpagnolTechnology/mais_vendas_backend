using Crosscutting.DTO.Proposal;
using Crosscutting.DTO.Sale;

namespace AppService.AppService.Interfaces
{
    public interface IProposalAppService
    {
        Task<IReadOnlyList<ProposalResponseDTO>> GetAllAsync(CancellationToken ct = default);
        Task<ProposalResponseDTO> GetResponseByIdAsync(int id, CancellationToken ct = default);
        Task<ProposalResponseDTO> CreateAsync(CreateProposalRequestDTO request, CancellationToken ct = default);
        Task<ProposalResponseDTO> UpdateAsync(int id, UpdateProposalRequestDTO request, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
        Task<ProposalResponseDTO> SendAsync(int id, CancellationToken ct = default);
        Task<ProposalResponseDTO> ApproveAsync(int id, CancellationToken ct = default);
        Task<SaleResponseDTO> ConvertToSaleAsync(int id, CancellationToken ct = default);
    }
}
