using MongoDB.Driver;
using Ordering.Application.ReadModel;

namespace Ordering.Infrastructure.ReadDatabase;

public class ReadDatabase : IReadDatabase
{
    public IMongoCollection<Order> OrderCollection { get; }

    public ReadDatabase(string connectionString)
    {
        var mongoClient = new MongoClient(connectionString);
        var mongoDatabase = mongoClient.GetDatabase("OrderingReadDB");
        
        OrderCollection = mongoDatabase.GetCollection<Order>("Orders");
    }
}