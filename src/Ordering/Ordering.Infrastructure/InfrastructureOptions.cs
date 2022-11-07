using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.Repositories;
using Ordering.Infrastructure.EventStore;
using Ordering.Infrastructure.EventStore.Repositories;

namespace Ordering.Infrastructure;

public interface IInfrastructureOptions
{
    void AddEventStore(string postgresConnectionString);
    void AddReadStore(string postgresConnectionString);
}

internal class InfrastructureOptions : IInfrastructureOptions
{
    private readonly IServiceCollection _services;

    public InfrastructureOptions(IServiceCollection services)
    {
        _services = services;
    }

    public void AddEventStore(string connectionString)
    {
        _services.AddSingleton(_ => new EventStore.EventStore(connectionString));
        AddRepositories();
    }

    public void AddReadStore(string connectionString)
    {
        throw new NotImplementedException();
    }

    private void AddRepositories()
    {
        _services.AddScoped<IOrderRepository, OrderRepository>();
    }
}