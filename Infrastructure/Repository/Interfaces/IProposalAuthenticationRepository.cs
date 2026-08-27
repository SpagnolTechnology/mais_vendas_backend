using Domain.Entity;
using Infrastructure.Base;

namespace Infrastructure.Repository.Interfaces
{
    public interface IProposalAuthenticationRepository : IGenericRepository<ProposalAuthenticationEntity>
    {
        Task<ProposalAuthenticationEntity?> GetByProposalUuidAsync(string proposalUuid, CancellationToken ct = default);
    }
}
