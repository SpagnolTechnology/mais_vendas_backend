using Domain.Entity;
using Infrastructure.Base;

namespace Infrastructure.Repository.Interfaces
{
    public interface IProductPhotoRepository : IGenericRepository<ProductPhotoEntity>
    {
        Task<IReadOnlyList<ProductPhotoEntity>> GetByProductIdAsync(int productId, CancellationToken ct = default);

        Task<ProductPhotoEntity?> GetByIdAndProductIdAsync(int id, int productId, CancellationToken ct = default);
    }
}
