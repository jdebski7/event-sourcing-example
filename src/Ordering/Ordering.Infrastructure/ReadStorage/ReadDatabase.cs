using MongoDB.Driver;
using Ordering.Application.ReadModel;

namespace Ordering.Infrastructure.ReadStorage;

public class ReadDatabase : IReadDatabase
{
    private const string OrdersCollectionName = "Orders";
    
    public IMongoCollection<Order> OrderCollection { get; }

    public ReadDatabase(string url, string database)
    {
        var mongoClient = new MongoClient(url);
        var mongoDatabase = mongoClient.GetDatabase(database);
        
        OrderCollection = mongoDatabase.GetCollection<Order>(OrdersCollectionName);
    }
}