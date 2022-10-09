using Ordering.Application.Common;
using Ordering.Application.ViewModels;
using Ordering.Contract.Queries.Orders;

namespace Ordering.Application.QueryHandlers.Orders;

public class OrderGetQueryHandler : IQueryHandler<OrderGetQuery, OrderViewModel>
{
    public Task<OrderViewModel> HandleAsync(OrderGetQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}