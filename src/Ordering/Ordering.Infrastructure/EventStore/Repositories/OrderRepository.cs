using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Model.Orders;
using Ordering.Domain.Repositories;
using Ordering.Infrastructure.EventStore.Model.OrderEvents;
using OrderEvent = Ordering.Domain.Model.Orders.OrderEvent;

namespace Ordering.Infrastructure.EventStore.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly EventStoreDbContext _eventStoreDbContext;

    public OrderRepository(EventStoreDbContext eventStoreDbContext)
    {
        _eventStoreDbContext = eventStoreDbContext;
    }

    public async Task AppendAsync(OrderEvent orderEvent, CancellationToken cancellationToken)
    {
        await _eventStoreDbContext.OrderEvents.AddAsync(new Model.OrderEvents.OrderEvent(
            correlationId: orderEvent.OrderId,
            version: orderEvent.OrderVersion,
            data: Map(orderEvent),
            DateTime.Now), cancellationToken);
    }

    public async Task<Order?> FirstOrDefaultByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var orderEvents = await _eventStoreDbContext.OrderEvents
            .AsNoTracking()
            .Where(e => e.CorrelationId == id)
            .OrderBy(e => e.Version)
            .ToListAsync(cancellationToken);

        if (orderEvents is not {Count: > 0})
        {
            return null;
        }

        return Order.ApplyEvents(id, orderEvents.Select<Model.OrderEvents.OrderEvent, OrderEvent>(e =>
        {
            return e.Data switch
            {
                OrderPlacedEventData => new OrderPlaced(e.CorrelationId, e.Version),
                OrderCancelledEventData => new OrderCancelled(e.CorrelationId, e.Version),
                OrderShippedEventData => new OrderShipped(e.CorrelationId, e.Version),
                _ => throw new ArgumentOutOfRangeException()
            };
        }).ToList());
    }

    private OrderEventData Map(OrderEvent orderEvent)
    {
        return orderEvent switch
        {
            OrderPlaced => new OrderPlacedEventData(),
            OrderCancelled => new OrderCancelledEventData(),
            OrderShipped => new OrderShippedEventData(),
            _ => throw new ArgumentOutOfRangeException(nameof(orderEvent))
        };
    }
}