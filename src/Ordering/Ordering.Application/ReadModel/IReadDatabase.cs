using MongoDB.Driver;

namespace Ordering.Application.ReadModel;

public interface IReadDatabase
{ 
    IMongoCollection<Order> OrderCollection { get; }
}