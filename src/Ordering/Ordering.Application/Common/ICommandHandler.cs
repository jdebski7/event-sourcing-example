namespace Ordering.Application.Common;

public interface ICommandHandler<in TCommand> where TCommand : class
{
    Task HandleAsync(TCommand command, CancellationToken cancellationToken);
}