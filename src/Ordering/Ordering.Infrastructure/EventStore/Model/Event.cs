namespace Ordering.Infrastructure.EventStore.Model;

public abstract class Event<TData> where TData : class
{
    public Guid CorrelationId { get; private init; }
    public ulong Version { get; private init; }
    public TData Data { get; private init; }
    public DateTime CreatedAt { get; private init; }

    protected Event(Guid correlationId, ulong version, TData data, DateTime createdAt)
    {
        CorrelationId = correlationId;
        Version = version;
        Data = data;
        CreatedAt = createdAt;
    }
}