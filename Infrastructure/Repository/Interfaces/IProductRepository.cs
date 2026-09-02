using Domain.Entity;
using Infrastructure.Base;

namespace Infrastructure.Repository.Interfaces
{
    public interface IProductRepository : IGenericRepository<ProductEntity>
    {
        Task<ProductEntity?> GetByIdWithStockAsync(int id, CancellationToken ct = default);
        Task<ProductEntity?> GetBySkuAsync(string sku, CancellationToken ct = default);
        Task<ProductEntity?> GetByEanAsync(string ean, CancellationToken ct = default);
    }
}
