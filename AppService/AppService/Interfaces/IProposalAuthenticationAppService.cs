namespace AppService.AppService.Interfaces
{
    public interface IProposalAuthenticationAppService
    {
        Task RegisterAsync(Guid proposalUuid, string companyCnpj, CancellationToken ct = default);

        Task<string?> GetCompanyCnpjByProposalUuidAsync(Guid proposalUuid, CancellationToken ct = default);
    }
}
