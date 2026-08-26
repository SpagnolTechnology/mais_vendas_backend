using Crosscutting.Enum;
using Domain.Entity;
using Infrastructure.Base;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Services
{
    public class ProductPurchaseEntryItemRepository : GenericRepository<ProductPurchaseEntryItemEntity, DatabaseContext>, IProductPurchaseEntryItemRepository
    {
        public ProductPurchaseEntryItemRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<decimal?> GetMaxUnitCostByProductIdAsync(int productId, CancellationToken ct = default)
        {
            return await _context.Set<ProductPurchaseEntryItemEntity>()
                .Join(
                    _context.Set<ProductPurchaseEntryEntity>(),
                    item => item.ProductPurchaseEntryId,
                    entry => entry.Id,
                    (item, entry) => new { item, entry })
                .Where(x => x.item.ProductId == productId &&
                            x.entry.Status == PurchaseEntryStatusEnum.Confirmed)
                .MaxAsync(x => (decimal?)x.item.UnitCost, ct);
        }
    }
}
