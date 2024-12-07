using System.Collections.ObjectModel;

namespace RestaurantManagement.Domain.Contracts;

public abstract class AggregateRoot : Entity
{
    private List<IDomainEvent>? _domainEvents;
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly()
        ?? ReadOnlyCollection<IDomainEvent>.Empty;

    protected void AddDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents ??= [];
        if (!_domainEvents.Contains(eventItem))
            _domainEvents.Add(eventItem);
    }

    protected void RemoveDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents?.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }

}
