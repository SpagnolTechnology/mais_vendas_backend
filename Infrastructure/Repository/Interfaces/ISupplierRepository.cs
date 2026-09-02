using Domain.Entity;
using Infrastructure.Base;

namespace Infrastructure.Repository.Interfaces
{
    public interface ISupplierRepository : IGenericRepository<SupplierEntity>
    {
        Task<SupplierEntity?> GetByDocumentAsync(string document, CancellationToken ct = default);
    }
}
