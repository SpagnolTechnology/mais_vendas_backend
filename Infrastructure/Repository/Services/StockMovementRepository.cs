using Domain.Entity;
using Infrastructure.Base;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Services
{
    public class StockMovementRepository : GenericRepository<StockMovementEntity, DatabaseContext>, IStockMovementRepository
    {
        public StockMovementRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<StockMovementEntity>> GetByProductIdAsync(int productId, CancellationToken ct = default)
        {
            return await _context.Set<StockMovementEntity>()
                .AsNoTracking()
                .Where(m => m.ProductId == productId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync(ct);
        }
    }
}
