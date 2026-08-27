using Domain.Entity;
using Infrastructure.Base;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Services
{
    public class ProductPhotoRepository : GenericRepository<ProductPhotoEntity, DatabaseContext>, IProductPhotoRepository
    {
        public ProductPhotoRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<ProductPhotoEntity>> GetByProductIdAsync(int productId, CancellationToken ct = default)
        {
            return await _context.Set<ProductPhotoEntity>()
                .AsNoTracking()
                .Where(p => p.ProductId == productId)
                .OrderBy(p => p.Id)
                .ToListAsync(ct);
        }

        public async Task<ProductPhotoEntity?> GetByIdAndProductIdAsync(int id, int productId, CancellationToken ct = default)
        {
            return await _context.Set<ProductPhotoEntity>()
                .FirstOrDefaultAsync(p => p.Id == id && p.ProductId == productId, ct);
        }
    }
}
