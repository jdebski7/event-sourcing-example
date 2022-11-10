namespace Ordering.Contract.States;

public class OrderState
{
    public Guid OrderId { get; }
    public ulong OrderVersion { get; }
    public string OrderStatus { get; }
    public DateTime OrderPlacedAt { get; }
    public DateTime? OrderCancelledAt { get; }
    public DateTime? OrderShippedAt { get; }
    public string? OrderShippingDestination { get; }

    public OrderState(Guid orderId, ulong orderVersion, string orderStatus, DateTime orderPlacedAt, 
        DateTime? orderCancelledAt, DateTime? orderShippedAt, string? orderShippingDestination)
    {
        OrderId = orderId;
        OrderVersion = orderVersion;
        OrderStatus = orderStatus;
        OrderPlacedAt = orderPlacedAt;
        OrderCancelledAt = orderCancelledAt;
        OrderShippedAt = orderShippedAt;
        OrderShippingDestination = orderShippingDestination;
    }
}