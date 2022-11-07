using MassTransit;
using Ordering.Contract.Queries.Orders;

namespace Ordering.Application.Consumers.QueryConsumers.Orders;

public class OrderListQueryConsumer : IConsumer<OrderListQuery>
{
    public Task Consume(ConsumeContext<OrderListQuery> context)
    {
        throw new NotImplementedException();
    }
}