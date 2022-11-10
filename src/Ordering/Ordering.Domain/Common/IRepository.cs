namespace Ordering.Domain.Common;

public interface IRepository<TAggregate, TEvent> 
    where TAggregate : class, IAggregate
    where TEvent : class, IEvent
{
    Task<TAggregate?> FirstOrDefaultByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AppendAsync(TEvent ev, CancellationToken cancellationToken);
}