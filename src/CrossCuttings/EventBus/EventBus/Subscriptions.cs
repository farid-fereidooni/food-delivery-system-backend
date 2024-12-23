using EventBus.Core;

namespace EventBus;

internal class Subscriptions : Dictionary<string, Queues>
{
    public void AddSubscription<TEvent>(string exchangeName, string queueName) where TEvent : Event
    {
        var eventType = typeof(TEvent);

        if (this.Any(a => a.Value.HasEvent(eventType)))
            throw new Exception($"Event {eventType.Name} already bound to an exchange and a queue");

        if (!ContainsKey(exchangeName))
        {
            if (this.Any(a => a.Value.HasQueue(queueName)))
                throw new Exception($"Queue {queueName} already bound to an exchange");

            var queueManager = new Queues();
            queueManager.AddQueue(queueName, eventType);
            this[exchangeName] = queueManager;
            return;
        }

        this[exchangeName].AddQueue(queueName, eventType);
    }
}

internal class Queues : Dictionary<string, Events>
{
    public void AddQueue(string queueName, Type eventType)
    {
        if (!ContainsKey(queueName))
        {
            var eventManager = new Events();
            eventManager.AddEvent(eventType);
            this[queueName] = eventManager;
            return;
        }

        this[queueName].AddEvent(eventType);
    }

    public bool HasQueue(string queueName)
    {
        return ContainsKey(queueName);
    }

    public Type GetEvent(string eventName)
    {
        return this.First(f => f.Value.ContainsKey(eventName)).Value[eventName];
    }

    public bool HasEvent(Type eventType) => Values.Any(a => a.HasEvent(eventType));
}

internal class Events : Dictionary<string, Type>
{
    public void AddEvent(Type eventType)
    {
       this[eventType.Name] = eventType;
    }

    public bool HasEvent(Type eventType) => ContainsKey(eventType.Name);
}
