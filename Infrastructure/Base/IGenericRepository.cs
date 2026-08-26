namespace Infrastructure.Base
{
    public interface IGenericRepository<TEntity>
    {
        Task<TEntity> AddAsync(TEntity entity, CancellationToken ct = default);
        Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> entities, CancellationToken ct = default);
        Task<TEntity> EditAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(IEnumerable<TEntity> entities);
        Task<IReadOnlyList<TEntity>> GetAllReadOnlyAsync(CancellationToken ct = default);
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity?> GetByIdAsync(string id);
    }
}
