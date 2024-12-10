using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using EventBus.Core;

namespace EventBus.Models;

public class EventLog
{
    protected EventLog()
    {

    }

    [SetsRequiredMembers]
    public EventLog(IEvent @event, string topic, Guid transactionId)
    {
        EventId = @event.Id;
        EventName = @event.GetType().Name;
        EventDate = @event.CreatedAt;
        TransactionId = transactionId;
        TimesSent = 0;
        Content = JsonSerializer.Serialize(@event, Constants.JsonSerializerOptions);
        State = EventStateEnum.NotPublished;
        Topic = topic;
    }

    public Guid EventId { get; private set; }
    public string EventName { get; private set; } = null!;
    public DateTime EventDate { get; set; }
    public Guid TransactionId { get; private set; }
    public int TimesSent { get; set; }
    internal string Content { get; private set; } = null!;
    public EventStateEnum State { get; set; }
    public string Topic { get; private set; } = null!;
}
