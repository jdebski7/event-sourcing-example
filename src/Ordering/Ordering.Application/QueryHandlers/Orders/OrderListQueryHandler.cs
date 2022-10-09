using Ordering.Application.Common;
using Ordering.Application.ViewModels;
using Ordering.Contract.Queries.Orders;

namespace Ordering.Application.QueryHandlers.Orders;

public class OrderListQueryHandler : IQueryHandler<OrderListQuery, IEnumerable<OrderViewModel>>
{
    public Task<IEnumerable<OrderViewModel>> HandleAsync(OrderListQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}