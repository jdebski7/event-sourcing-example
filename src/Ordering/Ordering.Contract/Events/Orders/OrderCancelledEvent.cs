namespace Ordering.Contract.Events.Orders;

public class OrderCancelledEvent
{
    public Guid OrderId { get; }
    public ulong OrderVersion { get; } 
    public DateTime OrderCancelledAt { get; }

    public OrderCancelledEvent(Guid orderId, ulong orderVersion, DateTime orderCancelledAt)
    {
        OrderId = orderId;
        OrderVersion = orderVersion;
        OrderCancelledAt = orderCancelledAt;
    }
}