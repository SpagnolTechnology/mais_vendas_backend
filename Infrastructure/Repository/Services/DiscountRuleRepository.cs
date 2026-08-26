using Domain.Entity;
using Infrastructure.Base;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Services
{
    public class DiscountRuleRepository : GenericRepository<DiscountRuleEntity, DatabaseContext>, IDiscountRuleRepository
    {
        public DiscountRuleRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<DiscountRuleEntity>> GetActiveRulesForUserAsync(string role, string email, CancellationToken ct = default)
        {
            return await _context.Set<DiscountRuleEntity>()
                .AsNoTracking()
                .Where(r => r.IsActive && (r.UserEmail == email || r.Role == role))
                .ToListAsync(ct);
        }
    }
}
