using Ordering.Domain.Common;

namespace Ordering.Domain.Model.Orders;

public abstract record OrderEvent(Guid OrderId, ulong OrderVersion) : IEvent;

public record OrderPlaced(Guid OrderId, ulong OrderVersion, DateTime PlacedAt) : OrderEvent(OrderId, OrderVersion);
public record OrderCancelled(Guid OrderId, ulong OrderVersion, DateTime CancelledAt) : OrderEvent(OrderId, OrderVersion);
public record OrderShipped(Guid OrderId, ulong OrderVersion, DateTime ShippedAt, string ShippingDestination) : OrderEvent(OrderId, OrderVersion);

internal static class OrderEventExtension
{
    internal static void Validate(this IEnumerable<OrderEvent> orderEvents, Guid orderId)
    {
        var any = orderEvents
            .Where((e, index) => e.OrderId != orderId || e.OrderVersion != (ulong) (index + 1))
            .Any();

        if (any)
        {
            throw new DomainException("Invalid order event");
        }
    }
}