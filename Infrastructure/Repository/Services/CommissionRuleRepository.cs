using Domain.Entity;
using Infrastructure.Base;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Services
{
    public class CommissionRuleRepository : GenericRepository<CommissionRuleEntity, DatabaseContext>, ICommissionRuleRepository
    {
        public CommissionRuleRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<CommissionRuleEntity?> GetGlobalActiveAsync(CancellationToken ct = default)
        {
            return await _context.Set<CommissionRuleEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.ProductId == null && r.IsActive, ct);
        }

        public async Task<CommissionRuleEntity?> GetByProductIdActiveAsync(int productId, CancellationToken ct = default)
        {
            return await _context.Set<CommissionRuleEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.ProductId == productId && r.IsActive, ct);
        }
    }
}
