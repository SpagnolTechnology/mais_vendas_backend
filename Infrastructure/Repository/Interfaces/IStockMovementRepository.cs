using Domain.Entity;
using Infrastructure.Base;

namespace Infrastructure.Repository.Interfaces
{
    public interface IStockMovementRepository : IGenericRepository<StockMovementEntity>
    {
        Task<IReadOnlyList<StockMovementEntity>> GetByProductIdAsync(int productId, CancellationToken ct = default);
    }
}
