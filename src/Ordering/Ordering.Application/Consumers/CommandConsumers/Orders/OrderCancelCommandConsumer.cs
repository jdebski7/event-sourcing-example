using MassTransit;
using Ordering.Contract.Commands.Orders;
using Ordering.Contract.Events.Orders;
using Ordering.Contract.States;
using Ordering.Domain.Repositories;

namespace Ordering.Application.Consumers.CommandConsumers.Orders;

public class OrderCancelCommandConsumer : IConsumer<OrderCancelCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderCancelCommandConsumer(IOrderRepository orderRepository, IPublishEndpoint publishEndpoint)
    {
        _orderRepository = orderRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<OrderCancelCommand> context)
    {
        var command = context.Message;
        var cancellationToken = context.CancellationToken;
        
        var order = await _orderRepository.FirstOrDefaultByIdAsync(command.OrderId, cancellationToken);
        if (order == null)
        {
            throw new Exception("Order doesn't exist");
        }

        var orderCancelled = order.Cancel();
        
        await _orderRepository.AppendAsync(orderCancelled, cancellationToken);
        
        await _publishEndpoint.Publish(new OrderCancelledEvent(
            orderId: orderCancelled.OrderId,
            orderVersion: orderCancelled.OrderVersion,
            orderCancelledAt: orderCancelled.CancelledAt), cancellationToken);
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