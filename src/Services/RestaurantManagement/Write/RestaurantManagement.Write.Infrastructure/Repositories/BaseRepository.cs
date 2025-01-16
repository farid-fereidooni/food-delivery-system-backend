using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Write.Domain.Contracts;
using RestaurantManagement.Write.Domain.Contracts.Repositories;

namespace RestaurantManagement.Write.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : AggregateRoot
{
    private readonly DbSet<TEntity> _dbSet;

    protected BaseRepository(DbSet<TEntity> dbSet)
    {
        _dbSet = dbSet;
    }

    public abstract Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(e => e.Id == id, cancellationToken);
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public virtual async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FindAsync([id], cancellationToken: cancellationToken);
        if (entity is not null)
            await DeleteAsync(entity, cancellationToken);
    }

    public virtual Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (!cancellationToken.IsCancellationRequested)
            _dbSet.Remove(entity);
        return Task.CompletedTask;
    }
}
