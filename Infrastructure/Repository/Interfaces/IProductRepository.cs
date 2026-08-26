using Domain.Entity;
using Infrastructure.Base;

namespace Infrastructure.Repository.Interfaces
{
    public interface IProductRepository : IGenericRepository<ProductEntity>
    {
        Task<ProductEntity?> GetByIdWithStockAsync(int id, CancellationToken ct = default);
    }
}
