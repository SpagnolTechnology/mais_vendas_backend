using Crosscutting.DTO.CommissionRule;

namespace AppService.AppService.Interfaces
{
    public interface ICommissionRuleAppService
    {
        Task<IReadOnlyList<CommissionRuleResponseDTO>> GetAllAsync(CancellationToken ct = default);
        Task<CommissionRuleResponseDTO> GetResponseByIdAsync(int id);
        Task<CommissionRuleResponseDTO> CreateAsync(CreateCommissionRuleRequestDTO request, CancellationToken ct = default);
        Task<CommissionRuleResponseDTO> UpdateAsync(int id, UpdateCommissionRuleRequestDTO request);
        Task DeleteAsync(int id);
    }
}
