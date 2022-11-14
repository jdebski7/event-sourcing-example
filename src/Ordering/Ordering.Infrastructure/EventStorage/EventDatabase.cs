using MongoDB.Driver;
using Ordering.Infrastructure.EventStorage.Model;

namespace Ordering.Infrastructure.EventStorage;

public class EventDatabase
{
    private const string OrderEventsCollectionName = "OrderEvents";
    
    public IMongoCollection<OrderEvent> OrderEventCollection { get; }

    public EventDatabase(string url, string database)
    {
        var mongoClient = new MongoClient(url);
        var mongoDatabase = mongoClient.GetDatabase(database);
        
        OrderEventCollection = mongoDatabase.GetCollection<OrderEvent>(OrderEventsCollectionName);

        // TODO: Move somewhere else
        // var indexKeysDefinition = Builders<OrderEvent>.IndexKeys.Combine("Version", "CorrelationId");
        // OrderEventCollection.Indexes.CreateOne(new CreateIndexModel<OrderEvent>(indexKeysDefinition));
    }
}