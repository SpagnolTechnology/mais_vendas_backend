using Domain.Entity;
using Infrastructure.Base;

namespace Infrastructure.Repository.Interfaces
{
    public interface ICommissionRuleRepository : IGenericRepository<CommissionRuleEntity>
    {
        Task<CommissionRuleEntity?> GetGlobalActiveAsync(CancellationToken ct = default);
        Task<CommissionRuleEntity?> GetByProductIdActiveAsync(int productId, CancellationToken ct = default);
    }
}
