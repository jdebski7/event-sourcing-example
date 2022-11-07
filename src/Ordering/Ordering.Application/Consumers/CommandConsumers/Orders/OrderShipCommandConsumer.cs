using MassTransit;
using Ordering.Contract.Commands.Orders;
using Ordering.Domain.Repositories;

namespace Ordering.Application.Consumers.CommandConsumers.Orders;

public class OrderShipCommandConsumer : IConsumer<OrderShipCommand>
{
    private readonly IOrderRepository _orderRepository;

    public OrderShipCommandConsumer(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
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
    }
}