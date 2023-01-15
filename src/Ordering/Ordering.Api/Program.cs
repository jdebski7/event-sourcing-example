using System.Reflection;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Ordering.Infrastructure.DependencyInjections;
using Ordering.Infrastructure.DependencyInjections.Settings;

var builder = WebApplication.CreateBuilder(args);

var mongoEventDatabase = new MongoEventDatabaseSettings();
builder.Configuration.Bind("MongoEventDatabase", mongoEventDatabase);

var mongoReadDatabase = new MongoReadDatabaseSettings();
builder.Configuration.Bind("MongoReadDatabase", mongoReadDatabase);

builder.Services.AddInfrastructure(b =>
{
    b.AddEventBus();
    b.AddMongoEventDatabase(mongoEventDatabase);
    b.AddMongoReadDatabase(mongoReadDatabase);
});

builder.Services.AddOpenTelemetry()
    .WithMetrics(b =>
    {
        b.SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService(Assembly.GetExecutingAssembly().GetName().Name!)
                .AddTelemetrySdk())
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