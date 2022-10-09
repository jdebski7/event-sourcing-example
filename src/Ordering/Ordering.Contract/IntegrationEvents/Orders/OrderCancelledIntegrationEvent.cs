namespace Ordering.Contract.IntegrationEvents.Orders;

public class OrderCancelledIntegrationEvent
{
    public Guid OrderId { get; }
    
    public OrderCancelledIntegrationEvent(Guid orderId)
    {
        OrderId = orderId;
    }

}