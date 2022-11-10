using Ordering.Application.Common;

namespace Ordering.Application.ReadModel;

public class Order : IReadModel
{
    public Guid Id { get; }
    public ulong Version { get; }
    public string Status { get; }
    public DateTime PlacedAt { get; }
    public DateTime? CancelledAt { get; }
    public DateTime? ShippedAt { get; }
    public string? ShippingDestination { get; }

    public Order(Guid id, ulong version, string status, DateTime placedAt, DateTime? cancelledAt, DateTime? shippedAt, 
        string? shippingDestination)
    {
        Id = id;
        Version = version;
        Status = status;
        PlacedAt = placedAt;
        CancelledAt = cancelledAt;
        ShippedAt = shippedAt;
        ShippingDestination = shippingDestination;
    }
}