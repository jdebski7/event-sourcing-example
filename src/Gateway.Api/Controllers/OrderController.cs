using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Ordering.Contract.Commands.Orders;
using Ordering.Contract.Queries.Orders;
using Ordering.Contract.ViewModels;

namespace Gateway.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IServiceProvider _serviceProvider;

    public OrderController(IPublishEndpoint publishEndpoint, IServiceProvider serviceProvider)
    {
        _publishEndpoint = publishEndpoint;
        _serviceProvider = serviceProvider;
    }

    [HttpPost("Place")]
    public async Task<Guid> Place(CancellationToken cancellationToken)
    {
        var orderId = Guid.NewGuid();
        await _publishEndpoint.Publish(new OrderPlaceCommand(orderId), cancellationToken);

        return orderId;
    }

    [HttpPost("Cancel")]
    public async Task Cancel(Guid orderId, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(new OrderCancelCommand(orderId), cancellationToken);
    }

    [HttpPost("Ship")]
    public async Task Ship(Guid orderId, string destination, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(new OrderShipCommand(orderId, destination), cancellationToken);
    }

    [HttpGet("Get")]
    public async Task<OrderViewModel> Get(Guid orderId, CancellationToken cancellationToken)
    {
        var query = new OrderGetQuery(orderId);

        var client = _serviceProvider.GetRequiredService<IRequestClient<OrderGetQuery>>();
        var response = await client.GetResponse<OrderViewModel>(query, cancellationToken);

        return response.Message;
    }

    [HttpGet("List")]
    public async Task<IEnumerable<OrderViewModel>> List(CancellationToken cancellationToken)
    {
        var query = new OrderListQuery();

        var client = _serviceProvider.GetRequiredService<IRequestClient<OrderListQuery>>();
        var response = await client.GetResponse<ListViewModel<OrderViewModel>>(query, cancellationToken);

        return response.Message.Items;
    }
}