using Domain.Entity;
using Infrastructure.Base;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Services
{
    public class StockAdjustmentRepository : GenericRepository<StockAdjustmentEntity, DatabaseContext>, IStockAdjustmentRepository
    {
        public StockAdjustmentRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<StockAdjustmentEntity?> GetByIdWithItemsAsync(int id, CancellationToken ct = default)
        {
            return await _context.Set<StockAdjustmentEntity>()
                .Include(a => a.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(a => a.Id == id, ct);
        }
    }
}
