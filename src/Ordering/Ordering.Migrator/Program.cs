using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Ordering.Infrastructure.EventStore;

namespace Ordering.Migrator;

internal static class Program
{
    private static void Main(string[] args)
    {
        using var host = CreateHostBuilder(args).Build();
        using var serviceScope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var migratorCommands = new[] {"migrate-latest"};

        if (args.Length > 0 && args.First() == "migrator")
        {
            migratorCommands = args.Skip(1).ToArray();
        }
        
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<EventStoreDbContext>();
        
        switch (migratorCommands[0])
        {
            case "migrate-latest":
                dbContext.Database.Migrate();
                break;
            case "drop-database":
                dbContext.Database.EnsureDeleted();
                break;
            default:
                Console.Error.WriteLine($"Unknown command ${args[0]}");
                break;
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
        {
            services.AddDbContext<EventStoreDbContext>(options =>
            {
                options.UseNpgsql(
                    "Host=localhost;Database=event-sourcing-example-write;",
                    sqlServer =>
                    {
                        sqlServer.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                    });
            });
        });
    }
}