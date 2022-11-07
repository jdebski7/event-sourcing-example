using MongoDB.Driver;
using Ordering.Infrastructure.EventStore.Model;

namespace Ordering.Infrastructure.EventStore;

public class EventStore
{
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDatabase;
    
    public IMongoCollection<OrderEvent> OrderEventCollection { get; }

    public EventStore(string connectionString)
    {
        _mongoClient = new MongoClient(connectionString);
        _mongoDatabase = _mongoClient.GetDatabase("OrderingEventsDB");
        OrderEventCollection = _mongoDatabase.GetCollection<OrderEvent>("OrderEvents");
    }
}