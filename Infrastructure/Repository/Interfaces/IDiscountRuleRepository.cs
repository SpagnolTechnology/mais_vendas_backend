using Domain.Entity;
using Infrastructure.Base;

namespace Infrastructure.Repository.Interfaces
{
    public interface IDiscountRuleRepository : IGenericRepository<DiscountRuleEntity>
    {
        Task<IReadOnlyList<DiscountRuleEntity>> GetActiveRulesForUserAsync(string role, string email, CancellationToken ct = default);
    }
}
