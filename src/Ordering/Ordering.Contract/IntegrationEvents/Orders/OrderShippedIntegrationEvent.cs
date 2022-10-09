namespace Ordering.Contract.IntegrationEvents.Orders;

public class OrderShippedIntegrationEvent
{
    public Guid OrderId { get; }

    public OrderShippedIntegrationEvent(Guid orderId)
    {
        OrderId = orderId;
    }
}