using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ordering.Infrastructure.EventStorage.Model;

public class OrderEvent
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; private set; }
    public Guid OrderId { get; }
    public ulong Version { get; }
    public OrderEventData Data { get; }
    public DateTime CreatedAt { get; }

    public OrderEvent(Guid orderId, ulong version, OrderEventData data, DateTime createdAt)
    {
        OrderId = orderId;
        Version = version;
        Data = data;
        CreatedAt = createdAt;
    }
}

public abstract class OrderEventData { }

public class OrderPlacedEventData : OrderEventData
{
    public DateTime PlacedAt { get; }

    public OrderPlacedEventData(DateTime placedAt)
    {
        PlacedAt = placedAt;
    }
}

public class OrderCancelledEventData : OrderEventData
{
    public DateTime CancelledAt { get; }

    public OrderCancelledEventData(DateTime cancelledAt)
    {
        CancelledAt = cancelledAt;
    }
}

public class OrderShippedEventData : OrderEventData
{
    public DateTime ShippedAt { get; }
    public string Destination { get; }

    public OrderShippedEventData(DateTime shippedAt, string destination)
    {
        ShippedAt = shippedAt;
        Destination = destination;
    }
}