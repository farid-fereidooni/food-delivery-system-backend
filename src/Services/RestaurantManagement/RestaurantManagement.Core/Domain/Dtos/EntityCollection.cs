using System.Collections;
using System.Diagnostics.CodeAnalysis;
using RestaurantManagement.Core.Domain.Contracts;

namespace RestaurantManagement.Core.Domain.Dtos;

public class EntityCollection<TEntity> : ICollection<TEntity>
    where TEntity : Entity
{
   private readonly Dictionary<Guid, TEntity> _dictionary = new ();
    public IEnumerator<TEntity> GetEnumerator()
    {
        return _dictionary.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(TEntity item)
    {
        _dictionary.Add(item.Id, item);
    }

    public void Clear()
    {
        _dictionary.Clear();
    }

    public bool Contains(TEntity item)
    {
        return _dictionary.ContainsKey(item.Id);
    }

    public bool ContainsBydId(Guid id)
    {
        return _dictionary.ContainsKey(id);
    }

    public TEntity GetById(Guid id) => _dictionary[id];

    public bool TryGetById(Guid id, [NotNullWhen(true)] out TEntity? item)
    {
        return _dictionary.TryGetValue(id, out item);
    }

    public void CopyTo(TEntity[] array, int arrayIndex)
    {
        _dictionary.Values.CopyTo(array, arrayIndex);
    }

    public bool Remove(TEntity item)
    {
        return _dictionary.Remove(item.Id);
    }

    public bool RemoveById(Guid id)
    {
        return _dictionary.Remove(id);
    }

    public int Count => _dictionary.Count;
    public bool IsReadOnly => false;
}
