namespace Ordering.Contract.Queries.Orders;

public class OrderGetQuery
{
    public Guid OrderId { get; }

    public OrderGetQuery(Guid orderId)
    {
        OrderId = orderId;
    }
}