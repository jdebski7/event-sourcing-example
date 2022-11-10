namespace Ordering.Contract.Events.Orders;

public class OrderPlacedEvent
{
    public Guid OrderId { get; }
    public ulong OrderVersion { get; } 
    public DateTime OrderPlacedAt { get; }

    public OrderPlacedEvent(Guid orderId, ulong orderVersion, DateTime orderPlacedAt)
    {
        OrderId = orderId;
        OrderVersion = orderVersion;
        OrderPlacedAt = orderPlacedAt;
    }
}