using Domain.Entity;
using Infrastructure.Base;

namespace Infrastructure.Repository.Interfaces
{
    public interface ISaleRepository : IGenericRepository<SaleEntity>
    {
        Task<SaleEntity?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);
    }
}
