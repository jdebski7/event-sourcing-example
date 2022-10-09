namespace Ordering.Contract.IntegrationEvents.Orders;

public class OrderPlacedIntegrationEvent
{
    public Guid OrderId { get; }
    
    public OrderPlacedIntegrationEvent(Guid orderId)
    {
        OrderId = orderId;
    }

}