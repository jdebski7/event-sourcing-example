using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.ReadModel;
using Ordering.Domain.Repositories;
using Ordering.Infrastructure.DependencyInjections.Settings;
using Ordering.Infrastructure.EventStorage;
using Ordering.Infrastructure.EventStorage.Repositories;
using Ordering.Infrastructure.ReadStorage;

namespace Ordering.Infrastructure.DependencyInjections;

public interface IInfrastructureOptions
{
    void AddMongoEventDatabase(MongoEventDatabaseSettings settings);
    void AddMongoReadDatabase(MongoReadDatabaseSettings settings);
}

internal class InfrastructureOptions : IInfrastructureOptions
{
    private readonly IServiceCollection _services;

    public InfrastructureOptions(IServiceCollection services)
    {
        _services = services;
    }

    public void AddMongoEventDatabase(MongoEventDatabaseSettings settings)
    {
        _services.AddSingleton(_ => new EventDatabase(settings.Url, settings.Database));
        AddRepositories();
    }

    public void AddMongoReadDatabase(MongoReadDatabaseSettings settings)
    {
        _services.AddSingleton<IReadDatabase>(_ => new ReadDatabase(settings.Url, settings.Database));
    }

    private void AddRepositories()
    {
        _services.AddScoped<IOrderRepository, OrderRepository>();
    }
}