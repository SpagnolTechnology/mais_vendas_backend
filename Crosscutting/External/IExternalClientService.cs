using Crosscutting.DTO.External;

namespace Crosscutting.External
{
    public interface IExternalClientService
    {
        Task<ExternalClientResponseDTO> GetClientAsync(string externalClientId, CancellationToken ct = default);
        Task ValidateClientAsync(string externalClientId, CancellationToken ct = default);
    }
}
