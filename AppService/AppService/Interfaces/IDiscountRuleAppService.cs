using Crosscutting.DTO.DiscountRule;

namespace AppService.AppService.Interfaces
{
    public interface IDiscountRuleAppService
    {
        Task<IReadOnlyList<DiscountRuleResponseDTO>> GetAllAsync(CancellationToken ct = default);
        Task<DiscountRuleResponseDTO> GetResponseByIdAsync(int id);
        Task<DiscountRuleResponseDTO> CreateAsync(CreateDiscountRuleRequestDTO request, CancellationToken ct = default);
        Task<DiscountRuleResponseDTO> UpdateAsync(int id, UpdateDiscountRuleRequestDTO request);
        Task DeleteAsync(int id);
    }
}
