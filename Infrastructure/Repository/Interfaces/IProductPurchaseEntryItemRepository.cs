using Domain.Entity;
using Infrastructure.Base;

namespace Infrastructure.Repository.Interfaces
{
    public interface IProductPurchaseEntryItemRepository : IGenericRepository<ProductPurchaseEntryItemEntity>
    {
        Task<decimal?> GetMaxUnitCostByProductIdAsync(int productId, CancellationToken ct = default);
    }
}
