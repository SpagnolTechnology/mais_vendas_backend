using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Base
{
    public abstract class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        protected readonly TContext _context;
        private readonly DbSet<TEntity> _dbSet;


        public GenericRepository(TContext context)
        {
            _dbSet = context.Set<TEntity>();
            _context = context;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entity, ct);
            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> entities, CancellationToken ct = default)
        {
            await _dbSet.AddRangeAsync(entities, ct);
            await _context.SaveChangesAsync();

            return entities;
        }

        public virtual async Task<TEntity> EditAsync(TEntity entity)
        {
            EntityEntry<TEntity> entry = _context.Entry(entity);

            if (entry == null)
                entry = _context.Attach(entity);

            entry.State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<IReadOnlyList<TEntity>> GetAllReadOnlyAsync(CancellationToken ct = default)
        {
            return await _dbSet.AsNoTracking().ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TEntity?> GetByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}
