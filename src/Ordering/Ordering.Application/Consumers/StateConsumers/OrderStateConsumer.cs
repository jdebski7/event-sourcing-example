using MassTransit;
using MongoDB.Driver;
using Ordering.Application.ReadModel;
using Ordering.Contract.States;

namespace Ordering.Application.Consumers.StateConsumers;

public class OrderStateConsumer : IConsumer<OrderState>
{
    private readonly IReadDatabase _readDatabase;

    public OrderStateConsumer(IReadDatabase readDatabase)
    {
        _readDatabase = readDatabase;
    }

    public async Task Consume(ConsumeContext<OrderState> context)
    {
        var message = context.Message;
        var cancellationToken = context.CancellationToken;

        var updatedOrder = new Order(
            id: message.OrderId,
            version: message.OrderVersion,
            status: message.OrderStatus,
            placedAt: message.OrderPlacedAt,
            cancelledAt: message.OrderCancelledAt,
            shippedAt: message.OrderShippedAt,
            shippingDestination: message.OrderShippingDestination);
        
        var order = await _readDatabase.OrderCollection  
            .Find(orderEvent => orderEvent.Id == message.OrderId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (order == null)
        {
            await _readDatabase.OrderCollection.InsertOneAsync(
                document: updatedOrder, 
                cancellationToken: cancellationToken);
        }
        else
        {
            await _readDatabase.OrderCollection.ReplaceOneAsync(
                filter: o => o.Id == order.Id && o.Version < message.OrderVersion, 
                replacement: updatedOrder,
                cancellationToken: cancellationToken);
        }
    }
}