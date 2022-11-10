using MassTransit;
using Ordering.Contract.Commands.Orders;
using Ordering.Contract.Events.Orders;
using Ordering.Contract.States;
using Ordering.Domain.Model.Orders;
using Ordering.Domain.Repositories;

namespace Ordering.Application.Consumers.CommandConsumers.Orders;

public class OrderPlaceCommandConsumer : IConsumer<OrderPlaceCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderPlaceCommandConsumer(IOrderRepository orderRepository, IPublishEndpoint publishEndpoint)
    {
        _orderRepository = orderRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<OrderPlaceCommand> context)
    {
        var command = context.Message;
        var cancellationToken = context.CancellationToken;

        var existingOrder = await _orderRepository.FirstOrDefaultByIdAsync(command.OrderId, cancellationToken);
        if (existingOrder != null)
        {
            throw new Exception("Order already exists");
        }

        var (order, orderPlaced) = Order.Place(command.OrderId);

        await _orderRepository.AppendAsync(orderPlaced, cancellationToken);
        
        await _publishEndpoint.Publish(new OrderPlacedEvent(
            orderId: orderPlaced.OrderId,
            orderVersion: orderPlaced.OrderVersion,
            orderPlacedAt: orderPlaced.PlacedAt), cancellationToken);
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