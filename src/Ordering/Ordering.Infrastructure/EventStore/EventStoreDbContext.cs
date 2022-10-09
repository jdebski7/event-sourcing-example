using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Ordering.Infrastructure.EventStore.Model.OrderEvents;

namespace Ordering.Infrastructure.EventStore;

public class EventStoreDbContext : DbContext
{
    public DbSet<OrderEvent> OrderEvents { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // mass-transit
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
        
        base.OnModelCreating(modelBuilder);
    }
}