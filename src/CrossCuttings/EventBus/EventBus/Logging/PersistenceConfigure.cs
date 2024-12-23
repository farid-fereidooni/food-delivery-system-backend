using EventBus.Models;
using Microsoft.EntityFrameworkCore;

namespace EventBus.Logging;

public static class PersistenceConfigure
{
    public static void AddEventLogs(this ModelBuilder modelBuilder)
    {
        var builder = modelBuilder.Entity<EventLog>();

        builder.HasKey(k => k.EventId);
        builder.HasIndex(i => new { i.State, i.TransactionId });

        builder.Property(p => p.EventName).HasMaxLength(70);
        builder.Property(p => p.Topic).HasMaxLength(50);
        builder.Property(p => p.Content);

        builder.Property(p => p.State).HasConversion<string>();
    }
}
