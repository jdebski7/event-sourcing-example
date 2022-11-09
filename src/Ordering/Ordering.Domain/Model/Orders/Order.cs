using Ordering.Domain.Common;

namespace Ordering.Domain.Model.Orders;

public class Order : Entity, IAggregate
{
    public Guid Id { get; }
    public ulong Version { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime PlacedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public DateTime? ShippedAt { get; private set; }
    public string? ShippingDestination { get; private set; }

    private Order(Guid id)
    {
        Id = id;
        Version = 0;
    }

    public static (Order, OrderPlaced) Place(Guid id)
    {
        var order = new Order(id);
        
        var orderPlaced = new OrderPlaced(order.Id, 1, DateTime.Now);
        order.Apply(orderPlaced);

        return (order, orderPlaced);
    }

    public OrderCancelled Cancel()
    {
        var orderCancelled = new OrderCancelled(Id, Version + 1, DateTime.Now);
        Apply(orderCancelled);

        return orderCancelled;
    }

    public OrderShipped Ship(string destination)
    {
        var orderShipped = new OrderShipped(Id, Version + 1, DateTime.Now, destination);
        Apply(orderShipped);

        return orderShipped;
    }

    private void Apply(OrderPlaced orderPlaced)
    {
        if (Version != 0 || Version != orderPlaced.OrderVersion - 1)
        {
            throw new DomainException("Invalid version");
        }

        Version = orderPlaced.OrderVersion;
        Status = OrderStatus.Placed();
        PlacedAt = orderPlaced.PlacedAt;
    }

    private void Apply(OrderCancelled orderCancelled)
    {
        if (Version != orderCancelled.OrderVersion - 1)
        {
            throw new DomainException("Invalid version");
        }
        
        if (Status == OrderStatus.Cancelled())
        {
            throw new DomainException("Order already cancelled");
        }

        if (Status == OrderStatus.Shipped())
        {
            throw new DomainException("Cannot cancel shipped order");
        }

        Version = orderCancelled.OrderVersion;
        Status = OrderStatus.Cancelled();
        CancelledAt = orderCancelled.CancelledAt;
    }

    private void Apply(OrderShipped orderShipped)
    {
        if (Version != orderShipped.OrderVersion - 1)
        {
            throw new DomainException("Invalid version");
        }
        
        if (Status == OrderStatus.Shipped())
        {
            throw new DomainException("Order already shipped");
        }

        if (Status == OrderStatus.Cancelled())
        {
            throw new DomainException("Cannot ship cancelled order");
        }
        
        Version = orderShipped.OrderVersion;
        Status = OrderStatus.Shipped();
        ShippedAt = orderShipped.ShippedAt;
        ShippingDestination = orderShipped.ShippingDestination;
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

