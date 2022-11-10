namespace Ordering.Domain.Model.Orders;

public readonly struct OrderStatus : IEquatable<OrderStatus>
{
    
    public override int GetHashCode()
    {
        return _status.GetHashCode();
    }

    private readonly string _status;

    private OrderStatus(string status)
    {
        _status = status;
    }

    public static OrderStatus Placed()
    {
        return new OrderStatus("Placed");
    }
    
    public static OrderStatus Cancelled()
    {
        return new OrderStatus("Cancelled");
    }
    
    public static OrderStatus Shipped()
    {
        return new OrderStatus("Shipped");
    }

    public override string ToString()
    {
        return _status;
    }
    
    public override bool Equals(object? obj)
    {
        return obj is OrderStatus other && Equals(other);
    }

    public bool Equals(OrderStatus other)
    {
        return _status == other._status;
    }
    
    public static bool operator ==(OrderStatus a, OrderStatus b)
    {
        return a.Equals(b);
    }
    
    public static bool operator !=(OrderStatus a, OrderStatus b) => !(a == b);
}