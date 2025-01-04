using MongoDB.Driver;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Query;

namespace RestaurantManagement.Infrastructure.Repositories.Query;

public abstract class BaseQueryRepository<T> : IQueryRepository<T> where T : StorableRoot
{
    private readonly IMongoCollection<T> _collection;

    public BaseQueryRepository(IMongoCollection<T> collection)
    {
        _collection = collection;
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(x => x.Id == id).AnyAsync(cancellationToken);

    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _collection.ReplaceOneAsync(c => c.Id == entity.Id, entity, cancellationToken: cancellationToken);
    }

    public async Task UpdateManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        foreach (var items in entities.Chunk(10))
        {
            await _collection.BulkWriteAsync(items.Select(
                item => new ReplaceOneModel<T>(Builders<T>.Filter.Where(w => w.Id == item.Id), item)),
                cancellationToken: cancellationToken);
        }
    }

    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _collection.DeleteOneAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }
}
