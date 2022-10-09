using Ordering.Domain.Model.Orders;

namespace Ordering.Infrastructure.EventStore.Model;

public abstract class Event<TData> where TData : class
{
    public Guid CorrelationId { get; }
    public ulong Version { get; }
    public TData Data { get; }
    public DateTime CreatedAt { get; }

    protected Event(Guid correlationId, ulong version, TData data, DateTime createdAt)
    {
        CorrelationId = correlationId;
        Version = version;
        Data = data;
        CreatedAt = createdAt;
    }
}