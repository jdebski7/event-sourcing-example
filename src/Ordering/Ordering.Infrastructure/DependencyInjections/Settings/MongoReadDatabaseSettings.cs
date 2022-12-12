namespace Ordering.Infrastructure.DependencyInjections.Settings;

#pragma warning disable CS8618

public class MongoReadDatabaseSettings
{
    public string Url { get; set; }
    public string Database { get; set; }
}