using Domain.Entity;
using Infrastructure.Base;

namespace Infrastructure.Repository.Interfaces
{
    public interface IProductStockRepository : IGenericRepository<ProductStockEntity>
    {
        Task<ProductStockEntity?> GetByProductIdAsync(int productId, CancellationToken ct = default);
    }
}
