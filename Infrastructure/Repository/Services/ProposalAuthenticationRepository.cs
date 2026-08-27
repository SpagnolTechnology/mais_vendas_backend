using Domain.Entity;
using Infrastructure.Base;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Services
{
    public class ProposalAuthenticationRepository : GenericRepository<ProposalAuthenticationEntity, DatabaseContextAdmin>, IProposalAuthenticationRepository
    {
        public ProposalAuthenticationRepository(DatabaseContextAdmin context) : base(context)
        {
        }

        public async Task<ProposalAuthenticationEntity?> GetByProposalUuidAsync(string proposalUuid, CancellationToken ct = default)
        {
            return await _context.ProposalAuthentication
                .FirstOrDefaultAsync(x => x.ProposalUuid == proposalUuid, ct);
        }
    }
}
