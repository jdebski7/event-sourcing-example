namespace Ordering.Infrastructure.EventStore.Model;

public abstract record OrderEventData;

public record OrderPlacedEventData(DateTime PlacedAt) : OrderEventData;
public record OrderCancelledEventData(DateTime CancelledAt) : OrderEventData;
public record OrderShippedEventData(DateTime ShippedAt, string Destination) : OrderEventData;

public class OrderEvent : Event<OrderEventData>
{
    public OrderEvent(Guid correlationId, ulong version, OrderEventData data, DateTime createdAt) : base(correlationId,
        version, data, createdAt)
    {
    }
}