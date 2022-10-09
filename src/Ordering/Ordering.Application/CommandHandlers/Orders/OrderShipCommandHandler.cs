using Ordering.Application.Common;
using Ordering.Contract.Commands.Orders;

namespace Ordering.Application.CommandHandlers.Orders;

public class OrderShipCommandHandler : ICommandHandler<OrderShipCommand>
{
    public Task HandleAsync(OrderShipCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}