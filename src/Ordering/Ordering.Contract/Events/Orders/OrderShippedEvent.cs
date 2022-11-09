namespace Ordering.Contract.Events.Orders;

public class OrderShippedEvent
{
    public Guid OrderId { get; }
    public ulong OrderVersion { get; } 
    public DateTime OrderShippedAt { get; }
    public string OrderShippingDestination { get; }

    public OrderShippedEvent(Guid orderId, ulong orderVersion, DateTime orderShippedAt, string orderShippingDestination)
    {
        OrderId = orderId;
        OrderVersion = orderVersion;
        OrderShippedAt = orderShippedAt;
        OrderShippingDestination = orderShippingDestination;
    }
}