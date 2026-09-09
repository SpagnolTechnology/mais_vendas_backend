using Crosscutting.Enum;
using Domain.Entity;
using Infrastructure.Base;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Services
{
    public class SaleRepository : GenericRepository<SaleEntity, DatabaseContext>, ISaleRepository
    {
        public SaleRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<SaleEntity?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default)
        {
            return await _context.Set<SaleEntity>()
                .Include(s => s.Items)
                .ThenInclude(i => i.Product)
                .Include(s => s.Commissions)
                .Include(s => s.PaymentCondition)
                .Include(s => s.Proposal)
                .FirstOrDefaultAsync(s => s.Id == id, ct);
        }

        public async Task<IReadOnlyList<SaleEntity>> GetConfirmedAsync(CancellationToken ct = default)
        {
            return await _context.Set<SaleEntity>()
                .AsNoTracking()
                .Where(s => s.Status == SaleStatusEnum.Confirmed)
                .OrderByDescending(s => s.SoldAt)
                .ThenByDescending(s => s.Id)
                .ToListAsync(ct);
        }
    }
}
