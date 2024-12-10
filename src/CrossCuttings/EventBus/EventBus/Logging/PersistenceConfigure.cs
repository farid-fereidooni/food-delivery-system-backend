using EventBus.Models;
using Microsoft.EntityFrameworkCore;

namespace EventBus.Logging;

public static class PersistenceConfigure
{
    public static void AddEventLogs(this ModelBuilder modelBuilder)
    {
        var builder = modelBuilder.Entity<EventLog>();

        builder.HasKey(k => k.EventId);
        builder.HasIndex(i => i.TransactionId);

        builder.Property(p => p.State).HasConversion<string>();
    }
}
