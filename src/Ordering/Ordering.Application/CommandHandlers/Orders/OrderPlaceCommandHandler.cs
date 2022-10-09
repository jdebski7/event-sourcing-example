using Ordering.Application.Common;
using Ordering.Contract.Commands.Orders;

namespace Ordering.Application.CommandHandlers.Orders;

public class OrderPlaceCommandHandler : ICommandHandler<OrderPlaceCommand>
{
    public Task HandleAsync(OrderPlaceCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}