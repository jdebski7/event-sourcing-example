namespace Ordering.Contract.Commands.Orders;

public class OrderPlaceCommand
{
    public Guid OrderId { get; }

    public OrderPlaceCommand(Guid orderId)
    {
        OrderId = orderId;
    }
}