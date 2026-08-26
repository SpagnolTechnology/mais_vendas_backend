using Domain.Entity;
using Infrastructure.Base;

namespace Infrastructure.Repository.Interfaces
{
    public interface IProposalRepository : IGenericRepository<ProposalEntity>
    {
        Task<ProposalEntity?> GetByIdWithItemsAsync(int id, CancellationToken ct = default);
        Task<decimal> GetReservedQuantityByProductIdAsync(int productId, CancellationToken ct = default);
    }
}
