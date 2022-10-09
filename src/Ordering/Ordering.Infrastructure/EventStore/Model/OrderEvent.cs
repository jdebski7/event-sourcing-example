namespace Ordering.Infrastructure.EventStore.Model;

public abstract record OrderEventData;

public record OrderPlacedEventData : OrderEventData;
public record OrderCancelledEventData : OrderEventData;
public record OrderShippedEventData : OrderEventData;

public class OrderEvent : Event<OrderEventData>
{
    public OrderEvent(Guid correlationId, ulong version, OrderEventData data, DateTime createdAt) : base(correlationId,
        version, data, createdAt)
    {
    }
}