using System.Reflection;
using MassTransit;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Ordering.Api;
using Ordering.Infrastructure.DependencyInjections;
using Ordering.Infrastructure.DependencyInjections.Settings;

var builder = WebApplication.CreateBuilder(args);

var mongoEventDatabase = new MongoEventDatabaseSettings();
builder.Configuration.Bind("MongoEventDatabase", mongoEventDatabase);

var mongoReadDatabase = new MongoReadDatabaseSettings();
builder.Configuration.Bind("MongoReadDatabase", mongoReadDatabase);

var rabbitMqSettings = new RabbitMqSettings();
builder.Configuration.Bind("RabbitMq", rabbitMqSettings);

builder.Services.AddInfrastructure(b =>
{
    b.AddMongoEventDatabase(mongoEventDatabase);
    b.AddMongoReadDatabase(mongoReadDatabase);
});

builder.Services.AddMassTransit(config =>
{
    config.AddConsumers(Assembly.Load("Ordering.Application"));
    config.UsingRabbitMq((c, mqConfig) =>
    {
        mqConfig.Host(rabbitMqSettings.Host, h =>
        {
            h.Username(rabbitMqSettings.Username);
            h.Password(rabbitMqSettings.Password);
        });
                
        mqConfig.ConfigureEndpoints(c);
        mqConfig.UseInMemoryOutbox();
    });
});

builder.Services.AddOpenTelemetry()
    .WithMetrics(b =>
    {
        b.SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService(Assembly.GetExecutingAssembly().GetName().Name!)
                .AddTelemetrySdk())
            .AddMeter("MassTransit")
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddOtlpExporter(otlpExporterOptions =>
            {
                builder.Configuration.GetSection("OpenTelemetry:OtlpExporter").Bind(otlpExporterOptions);
            });
    })
    .WithTracing(b =>
    {
        b.SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService(Assembly.GetExecutingAssembly().GetName().Name!)
                .AddTelemetrySdk())
            .AddSource("MassTransit")
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(otlpExporterOptions =>
            {
                builder.Configuration.GetSection("OpenTelemetry:OtlpExporter").Bind(otlpExporterOptions);
            });
    }).StartWithHost();

builder.Logging.AddOpenTelemetry(c =>
{
    c.SetResourceBuilder(ResourceBuilder.CreateDefault()
            .AddService(Assembly.GetExecutingAssembly().GetName().Name!)
            .AddTelemetrySdk())
        .AddOtlpExporter(otlpExporterOptions =>
        {
            builder.Configuration.GetSection("OpenTelemetry:OtlpExporter").Bind(otlpExporterOptions);
        });
});

var app = builder.Build();

app.Run();