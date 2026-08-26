using Domain.Entity;
using Infrastructure.Base;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Services
{
    public class ProductStockRepository : GenericRepository<ProductStockEntity, DatabaseContext>, IProductStockRepository
    {
        public ProductStockRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<ProductStockEntity?> GetByProductIdAsync(int productId, CancellationToken ct = default)
        {
            return await _context.Set<ProductStockEntity>()
                .FirstOrDefaultAsync(s => s.ProductId == productId, ct);
        }
    }
}
