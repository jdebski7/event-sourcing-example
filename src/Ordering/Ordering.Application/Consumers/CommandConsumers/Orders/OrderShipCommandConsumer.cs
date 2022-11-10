using MassTransit;
using Ordering.Contract.Commands.Orders;
using Ordering.Contract.Events.Orders;
using Ordering.Contract.States;
using Ordering.Domain.Repositories;

namespace Ordering.Application.Consumers.CommandConsumers.Orders;

public class OrderShipCommandConsumer : IConsumer<OrderShipCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderShipCommandConsumer(IOrderRepository orderRepository, IPublishEndpoint publishEndpoint)
    {
        _orderRepository = orderRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<OrderShipCommand> context)
    {
        var command = context.Message;
        var cancellationToken = context.CancellationToken;
        
        var order = await _orderRepository.FirstOrDefaultByIdAsync(command.OrderId, cancellationToken);
        if (order == null)
        {
            throw new Exception("Order doesn't exist");
        }

        var orderShipped = order.Ship(command.Destination);
        
        await _orderRepository.AppendAsync(orderShipped, cancellationToken);
        
        await _publishEndpoint.Publish(new OrderShippedEvent(
            orderId: orderShipped.OrderId,
            orderVersion: orderShipped.OrderVersion,
            orderShippedAt: orderShipped.ShippedAt,
            orderShippingDestination: orderShipped.ShippingDestination), cancellationToken);
        await _publishEndpoint.Publish(new OrderState(
            orderId: order.Id,
            orderVersion: order.Version,
            orderStatus: order.Status.ToString(),
            orderPlacedAt: order.PlacedAt,
            orderCancelledAt: order.CancelledAt,
            orderShippedAt: order.ShippedAt,
            orderShippingDestination: order.ShippingDestination), cancellationToken);
    }
}