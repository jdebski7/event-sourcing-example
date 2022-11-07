using MassTransit;
using Ordering.Contract.Commands.Orders;
using Ordering.Domain.Model.Orders;
using Ordering.Domain.Repositories;

namespace Ordering.Application.Consumers.CommandConsumers.Orders;

public class OrderPlaceCommandConsumer : IConsumer<OrderPlaceCommand>
{
    private readonly IOrderRepository _orderRepository;

    public OrderPlaceCommandConsumer(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Consume(ConsumeContext<OrderPlaceCommand> context)
    {
        var command = context.Message;
        var cancellationToken = context.CancellationToken;

        var order = await _orderRepository.FirstOrDefaultByIdAsync(command.OrderId, cancellationToken);
        if (order != null)
        {
            throw new Exception("Order already exists");
        }

        var (_, orderPlaced) = Order.Place(command.OrderId);

        await _orderRepository.AppendAsync(orderPlaced, cancellationToken);
    }
}