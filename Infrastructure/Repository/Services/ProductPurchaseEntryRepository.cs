using Domain.Entity;
using Infrastructure.Base;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Services
{
    public class ProductPurchaseEntryRepository : GenericRepository<ProductPurchaseEntryEntity, DatabaseContext>, IProductPurchaseEntryRepository
    {
        public ProductPurchaseEntryRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<ProductPurchaseEntryEntity?> GetByIdWithItemsAsync(int id, CancellationToken ct = default)
        {
            return await _context.Set<ProductPurchaseEntryEntity>()
                .Include(e => e.Items)
                .ThenInclude(i => i.Product)
                .Include(e => e.Supplier)
                .FirstOrDefaultAsync(e => e.Id == id, ct);
        }
    }
}
