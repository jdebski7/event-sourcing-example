using MongoDB.Driver;
using Ordering.Infrastructure.EventStore.Model;

namespace Ordering.Infrastructure.EventStore;

public class EventStore
{
    public IMongoCollection<OrderEvent> OrderEventCollection { get; }

    public EventStore(string connectionString)
    {
        var mongoClient = new MongoClient(connectionString);
        var mongoDatabase = mongoClient.GetDatabase("OrderingEventsDB");
        
        OrderEventCollection = mongoDatabase.GetCollection<OrderEvent>("OrderEvents");

        // TODO: Move somewhere else
        // var indexKeysDefinition = Builders<OrderEvent>.IndexKeys.Combine("Version", "CorrelationId");
        // OrderEventCollection.Indexes.CreateOne(new CreateIndexModel<OrderEvent>(indexKeysDefinition));
    }
}