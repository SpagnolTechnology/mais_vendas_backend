using Domain.Entity;
using Infrastructure.Base;

namespace Infrastructure.Repository.Interfaces
{
    public interface IDiscountRepository : IGenericRepository<DiscountEntity>
    {
        Task<IReadOnlyList<DiscountEntity>> GetByProposalIdAsync(int proposalId, CancellationToken ct = default);
        Task<IReadOnlyList<DiscountEntity>> GetBySaleIdAsync(int saleId, CancellationToken ct = default);
    }
}
