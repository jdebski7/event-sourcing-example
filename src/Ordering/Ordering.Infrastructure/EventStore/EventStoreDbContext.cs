using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Ordering.Infrastructure.EventStore.Model;

namespace Ordering.Infrastructure.EventStore;

public class EventStoreDbContext : DbContext
{
    public DbSet<OrderEvent> OrderEvents { get; private set; }

    public EventStoreDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // mass-transit
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
        
        base.OnModelCreating(modelBuilder);
    }
}