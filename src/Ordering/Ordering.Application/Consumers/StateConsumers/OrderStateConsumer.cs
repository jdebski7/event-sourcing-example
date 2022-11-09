using MassTransit;
using Ordering.Contract.States;

namespace Ordering.Application.Consumers.StateConsumers;

public class OrderStateConsumer : IConsumer<OrderState>
{
    public Task Consume(ConsumeContext<OrderState> context)
    {
        throw new NotImplementedException();
    }
}