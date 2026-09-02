using Crosscutting.Helpers;
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

        public async Task<ProductEntity?> GetBySkuAsync(string sku, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(sku))
                return null;

            return await _context.Set<ProductEntity>()
                .FirstOrDefaultAsync(p => p.Sku == sku.Trim(), ct);
        }

        public async Task<ProductEntity?> GetByEanAsync(string ean, CancellationToken ct = default)
        {
            string normalizedEan = DocumentHelper.NormalizeDigits(ean);
            if (string.IsNullOrEmpty(normalizedEan))
                return null;

            return await _context.Set<ProductEntity>()
                .FirstOrDefaultAsync(p => p.Ean != null && p.Ean == normalizedEan, ct);
        }
    }
}
