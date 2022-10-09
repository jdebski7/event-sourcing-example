using Ordering.Domain.Common;
using Ordering.Domain.Model.Orders;

namespace Ordering.Domain.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task AppendAsync(OrderEvent orderEvent, CancellationToken cancellationToken);
}