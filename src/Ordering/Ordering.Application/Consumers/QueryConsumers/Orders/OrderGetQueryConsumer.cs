using MassTransit;
using Ordering.Contract.Queries.Orders;

namespace Ordering.Application.Consumers.QueryConsumers.Orders;

public class OrderGetQueryConsumer : IConsumer<OrderGetQuery>
{
    public Task Consume(ConsumeContext<OrderGetQuery> context)
    {
        throw new NotImplementedException();
    }
}