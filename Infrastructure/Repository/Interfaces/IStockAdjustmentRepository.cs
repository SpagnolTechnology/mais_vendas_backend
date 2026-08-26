using Domain.Entity;
using Infrastructure.Base;

namespace Infrastructure.Repository.Interfaces
{
    public interface IStockAdjustmentRepository : IGenericRepository<StockAdjustmentEntity>
    {
        Task<StockAdjustmentEntity?> GetByIdWithItemsAsync(int id, CancellationToken ct = default);
    }
}
