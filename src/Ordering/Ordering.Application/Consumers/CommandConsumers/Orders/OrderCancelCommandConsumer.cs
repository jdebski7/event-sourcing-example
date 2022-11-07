using MassTransit;
using Ordering.Contract.Commands.Orders;
using Ordering.Domain.Repositories;

namespace Ordering.Application.Consumers.CommandConsumers.Orders;

public class OrderCancelCommandConsumer : IConsumer<OrderCancelCommand>
{
    private readonly IOrderRepository _orderRepository;

    public OrderCancelCommandConsumer(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
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
    }
}