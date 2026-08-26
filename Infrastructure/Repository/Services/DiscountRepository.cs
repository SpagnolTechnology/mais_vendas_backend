using Domain.Entity;
using Infrastructure.Base;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Services
{
    public class DiscountRepository : GenericRepository<DiscountEntity, DatabaseContext>, IDiscountRepository
    {
        public DiscountRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<DiscountEntity>> GetByProposalIdAsync(int proposalId, CancellationToken ct = default)
        {
            return await _context.Set<DiscountEntity>()
                .AsNoTracking()
                .Where(d => d.ProposalId == proposalId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<DiscountEntity>> GetBySaleIdAsync(int saleId, CancellationToken ct = default)
        {
            return await _context.Set<DiscountEntity>()
                .AsNoTracking()
                .Where(d => d.SaleId == saleId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync(ct);
        }
    }
}
