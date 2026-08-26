using Crosscutting.Enum;
using Domain.Entity;
using Infrastructure.Base;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Services
{
    public class ProposalRepository : GenericRepository<ProposalEntity, DatabaseContext>, IProposalRepository
    {
        public ProposalRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<ProposalEntity?> GetByIdWithItemsAsync(int id, CancellationToken ct = default)
        {
            return await _context.Set<ProposalEntity>()
                .Include(p => p.Items)
                .ThenInclude(i => i.Product)
                .Include(p => p.PaymentCondition)
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task<decimal> GetReservedQuantityByProductIdAsync(int productId, CancellationToken ct = default)
        {
            var activeStatuses = new[]
            {
                ProposalStatusEnum.Draft,
                ProposalStatusEnum.Sent,
                ProposalStatusEnum.Approved
            };

            return await _context.Set<ProposalItemEntity>()
                .Join(
                    _context.Set<ProposalEntity>(),
                    item => item.ProposalId,
                    proposal => proposal.Id,
                    (item, proposal) => new { item, proposal })
                .Where(x => x.item.ProductId == productId &&
                            activeStatuses.Contains(x.proposal.Status))
                .SumAsync(x => x.item.Quantity, ct);
        }
    }
}
