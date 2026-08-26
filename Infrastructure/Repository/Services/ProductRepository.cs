using Domain.Entity;
using Infrastructure.Base;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Services
{
    public class ProductRepository : GenericRepository<ProductEntity, DatabaseContext>, IProductRepository
    {
        public ProductRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<ProductEntity?> GetByIdWithStockAsync(int id, CancellationToken ct = default)
        {
            return await _context.Set<ProductEntity>()
                .Include(p => p.ProductStock)
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }
    }
}
