using MongoDB.Driver;
using RestaurantManagement.Read.Domain.Contracts;
using RestaurantManagement.Read.Domain.Contracts.Repositories;

namespace RestaurantManagement.Read.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IRepository<T> where T : StorableRoot
{
    private readonly IMongoCollection<T> _collection;

    public BaseRepository(IMongoCollection<T> collection)
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
