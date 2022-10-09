using Ordering.Application.Common;
using Ordering.Contract.Commands.Orders;

namespace Ordering.Application.CommandHandlers.Orders;

public class OrderCancelCommandHandler : ICommandHandler<OrderCancelCommand>
{
    public Task HandleAsync(OrderCancelCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}