using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common;
using Ordering.Application.ViewModels;
using Ordering.Contract.Commands.Orders;
using Ordering.Contract.Queries.Orders;

namespace Ordering.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;

    public OrderController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [HttpPost("Place")]
    public Task Place(OrderPlaceCommand command, CancellationToken cancellationToken)
    {
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<OrderPlaceCommand>>();
        return handler.HandleAsync(command, cancellationToken);
    }

    [HttpPost("Cancel")]
    public Task Cancel(OrderCancelCommand command, CancellationToken cancellationToken)
    {
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<OrderCancelCommand>>();
        return handler.HandleAsync(command, cancellationToken);   
    }

    [HttpPost("Ship")]
    public Task Ship(OrderShipCommand command, CancellationToken cancellationToken)
    {
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<OrderShipCommand>>();
        return handler.HandleAsync(command, cancellationToken);
    }

    [HttpGet("Get")]
    public Task<OrderViewModel> Get(OrderGetQuery query, CancellationToken cancellationToken)
    {
        var handler = _serviceProvider.GetRequiredService<IQueryHandler<OrderGetQuery, OrderViewModel>>();
        return handler.HandleAsync(query, cancellationToken);
    }

    [HttpGet("List")]
    public Task<IEnumerable<OrderViewModel>> List(OrderListQuery query, CancellationToken cancellationToken)
    {
        var handler = _serviceProvider.GetRequiredService<IQueryHandler<OrderListQuery, IEnumerable<OrderViewModel>>>();
        return handler.HandleAsync(query, cancellationToken);
    }
}