using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Core.Domain.Contracts;

namespace RestaurantManagement.Infrastructure.Repositories.Command;

public abstract class BaseCommandRepository<TEntity> : ICommandRepository<TEntity> where TEntity : AggregateRoot
{
    private readonly DbSet<TEntity> _dbSet;

    protected BaseCommandRepository(DbSet<TEntity> dbSet)
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
