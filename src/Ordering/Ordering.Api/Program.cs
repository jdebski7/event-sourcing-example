using System.Reflection;
using MassTransit;
using Ordering.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(config =>
{
    config.AddConsumers(Assembly.Load("Ordering.Application"));
    config.UsingRabbitMq((c, cfg) =>
    {
        cfg.ConfigureEndpoints(c);
        cfg.UseInMemoryOutbox();
    });
});

builder.Services.AddInfrastructure(b =>
{
    b.AddEventStore("mongodb://localhost:27017");
    b.AddReadStore("mongodb://localhost:27017");
});

var app = builder.Build();

app.Run();