using Domain.Entity;
using Infrastructure.Base;

namespace Infrastructure.Repository.Interfaces
{
    public interface IProductPurchaseEntryRepository : IGenericRepository<ProductPurchaseEntryEntity>
    {
        Task<ProductPurchaseEntryEntity?> GetByIdWithItemsAsync(int id, CancellationToken ct = default);
    }
}
