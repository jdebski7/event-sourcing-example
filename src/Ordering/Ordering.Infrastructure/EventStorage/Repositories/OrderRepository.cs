using MongoDB.Driver;
using Ordering.Domain.Model.Orders;
using Ordering.Domain.Repositories;
using Ordering.Infrastructure.EventStorage.Model;
using OrderEvent = Ordering.Infrastructure.EventStorage.Model.OrderEvent;
using DomainOrderEvent = Ordering.Domain.Model.Orders.OrderEvent;

namespace Ordering.Infrastructure.EventStorage.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly EventDatabase _eventDatabase;

    public OrderRepository(EventDatabase eventDatabase)
    {
        _eventDatabase = eventDatabase;
    }

    public async Task AppendAsync(DomainOrderEvent orderEvent, CancellationToken cancellationToken)
    {
        await _eventDatabase.OrderEventCollection.InsertOneAsync(new OrderEvent(
            orderId: orderEvent.OrderId,
            version: orderEvent.OrderVersion,
            data: Map(orderEvent),
            createdAt: DateTime.Now), cancellationToken: cancellationToken);
    }

    public async Task<Order?> FirstOrDefaultByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var orderEvents = await _eventDatabase.OrderEventCollection
            .Find(orderEvent => orderEvent.OrderId == id)
            .SortBy(orderEvent => orderEvent.Version)
            .ToListAsync(cancellationToken: cancellationToken);
        
        if (orderEvents is not {Count: > 0})
        {
            return null;
        }

        return Order.ApplyEvents(id, orderEvents.Select<OrderEvent, DomainOrderEvent>(e =>
        {
            return e.Data switch
            {
                OrderPlacedEventData data => new OrderPlaced(e.OrderId, e.Version, data.PlacedAt),
                OrderCancelledEventData data => new OrderCancelled(e.OrderId, e.Version, data.CancelledAt),
                OrderShippedEventData data => new OrderShipped(e.OrderId, e.Version, data.ShippedAt, data.Destination),
                _ => throw new ArgumentOutOfRangeException()
            };
        }).ToList());
    }

    private static OrderEventData Map(Domain.Model.Orders.OrderEvent orderEvent)
    {
        return orderEvent switch
        {
            OrderPlaced ev => new OrderPlacedEventData(ev.PlacedAt),
            OrderCancelled ev => new OrderCancelledEventData(ev.CancelledAt),
            OrderShipped ev => new OrderShippedEventData(ev.ShippedAt, ev.ShippingDestination),
            _ => throw new ArgumentOutOfRangeException(nameof(orderEvent))
        };
    }
}