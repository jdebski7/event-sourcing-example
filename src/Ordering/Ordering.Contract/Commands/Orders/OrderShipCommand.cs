namespace Ordering.Contract.Commands.Orders;

public class OrderShipCommand
{
    public Guid OrderId { get; }
    public string Destination { get; }

    public OrderShipCommand(Guid orderId, string destination)
    {
        OrderId = orderId;
        Destination = destination;
    }
}