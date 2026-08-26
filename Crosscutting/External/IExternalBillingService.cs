using Crosscutting.DTO.External;

namespace Crosscutting.External
{
    public interface IExternalBillingService
    {
        Task<string> CreateBillingAsync(CreateExternalBillingRequestDTO request, CancellationToken ct = default);
    }
}
