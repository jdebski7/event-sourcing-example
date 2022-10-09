using Ordering.Domain.Common;

namespace Ordering.Domain.Model.Orders;

public class Order : Entity, IAggregate
{
    public Guid Id { get; }
    public OrderStatus Status { get; private set; }
    public ulong Version { get; private set; }

    public Order(Guid id)
    {
        Id = id;
        Version = 0;
    }

    public OrderPlaced Place()
    {
        var orderPlaced = new OrderPlaced(Id, 1);
        Apply(orderPlaced);

        return orderPlaced;
    }

    public OrderCancelled Cancel()
    {
        var orderCancelled = new OrderCancelled(Id, Version + 1);
        Apply(orderCancelled);

        return orderCancelled;
    }

    public OrderShipped Ship()
    {
        var orderShipped = new OrderShipped(Id, Version + 1);
        Apply(orderShipped);

        return orderShipped;
    }

    private void Apply(OrderPlaced orderPlaced)
    {
        if (Version != 0 || Version != orderPlaced.OrderVersion - 1)
        {
            throw new DomainException("Invalid version");
        }

        Status = OrderStatus.Placed;
        Version = orderPlaced.OrderVersion;
    }

    private void Apply(OrderCancelled orderCancelled)
    {
        if (Version != orderCancelled.OrderVersion - 1)
        {
            throw new DomainException("Invalid version");
        }
        
        if (Status == OrderStatus.Cancelled)
        {
            throw new DomainException("Order already cancelled");
        }

        if (Status == OrderStatus.Shipped)
        {
            throw new DomainException("Cannot cancel shipped order");
        }

        Status = OrderStatus.Cancelled;
        Version = orderCancelled.OrderVersion;
    }

    private void Apply(OrderShipped orderShipped)
    {
        if (Version != orderShipped.OrderVersion - 1)
        {
            throw new DomainException("Invalid version");
        }
        
        if (Status == OrderStatus.Shipped)
        {
            throw new DomainException("Order already shipped");
        }

        if (Status == OrderStatus.Cancelled)
        {
            throw new DomainException("Cannot ship cancelled order");
        }
        
        Status = OrderStatus.Shipped;
        Version = orderShipped.OrderVersion;
    }

    public static Order ApplyEvents(Guid orderId, IList<OrderEvent> orderEvents)
    {
        orderEvents.Validate(orderId);
        var order = new Order(orderId);

        foreach (var orderEvent in orderEvents)
        {
            switch (orderEvent)
            {
                case OrderPlaced orderPlaced:
                    order.Apply(orderPlaced);
                    break;
                case OrderCancelled orderCancelled:
                    order.Apply(orderCancelled);
                    break;
                case OrderShipped orderShipped:
                    order.Apply(orderShipped);
                    break;
                default:
                    throw new DomainException("Unknown order event");
            }
        }

        return order;
    }
}

