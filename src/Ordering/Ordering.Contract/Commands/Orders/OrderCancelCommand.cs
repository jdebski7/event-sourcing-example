namespace Ordering.Contract.Commands.Orders;

public class OrderCancelCommand
{
    public Guid OrderId { get; }

    public OrderCancelCommand(Guid orderId)
    {
        OrderId = orderId;
    }
}