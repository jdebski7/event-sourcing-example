namespace Ordering.Contract.Commands.Orders;

public class OrderShipCommand
{
    public Guid OrderId { get; }

    public OrderShipCommand(Guid orderId)
    {
        OrderId = orderId;
    }
}