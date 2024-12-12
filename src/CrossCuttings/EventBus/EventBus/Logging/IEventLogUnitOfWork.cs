using EventBus.Models;
using Microsoft.EntityFrameworkCore;

namespace EventBus.Logging;

public interface IEventLogUnitOfWork
{
    DbSet<EventLog> EventLogs { get; }
    Task SaveAsync(CancellationToken cancellationToken = default);
}
