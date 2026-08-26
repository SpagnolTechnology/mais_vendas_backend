using Crosscutting.DTO.PaymentCondition;

namespace AppService.AppService.Interfaces
{
    public interface IPaymentConditionAppService
    {
        Task<IReadOnlyList<PaymentConditionResponseDTO>> GetAllAsync(CancellationToken ct = default);
        Task<PaymentConditionResponseDTO> GetResponseByIdAsync(int id);
        Task<PaymentConditionResponseDTO> CreateAsync(CreatePaymentConditionRequestDTO request, CancellationToken ct = default);
        Task<PaymentConditionResponseDTO> UpdateAsync(int id, UpdatePaymentConditionRequestDTO request);
        Task DeleteAsync(int id);
    }
}
