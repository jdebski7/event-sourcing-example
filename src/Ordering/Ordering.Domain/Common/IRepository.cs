namespace Ordering.Domain.Common;

public interface IRepository<TAggregate> where TAggregate : class, IAggregate
{
    Task<TAggregate?> FirstOrDefaultByIdAsync(Guid id, CancellationToken cancellationToken);
}