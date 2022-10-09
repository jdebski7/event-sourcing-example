using MassTransit;
using Microsoft.EntityFrameworkCore;
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

    public void AddEventStore(string postgresConnectionString)
    {
        _services.AddDbContext<EventStoreDbContext>(options =>
        {
            options.UseNpgsql(postgresConnectionString);
        });
        
        AddRepositories();
            
        _services.AddMassTransit(c =>
        {
            c.AddEntityFrameworkOutbox<EventStoreDbContext>(o =>
            {
                o.UsePostgres();
                o.UseBusOutbox();
            });
    
            c.UsingRabbitMq((_, config) =>
            {
                config.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });
        });
    }

    public void AddReadStore(string postgresConnectionString)
    {
        throw new NotImplementedException();
    }

    private void AddRepositories()
    {
        _services.AddScoped<IOrderRepository, OrderRepository>();
    }
}