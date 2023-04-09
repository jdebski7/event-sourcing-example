using System.Reflection;
using MassTransit;
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
    b.AddMongoEventDatabase(mongoEventDatabase);
    b.AddMongoReadDatabase(mongoReadDatabase);
});

builder.Services.AddMassTransit(config =>
{
    config.AddConsumers(Assembly.Load("Ordering.Application"));
    config.UsingRabbitMq((c, cfg) =>
    {
        cfg.Host("cluster.rabbitmq.svc.cluster.local", h =>
        {
            h.Username("default_user_DiWVUL01FFVg0xtGWhW");
            h.Password("z2yGSiiN4m2grNWdkM-DZhrNF8aRCKKr");
        });
                
        cfg.ConfigureEndpoints(c);
        cfg.UseInMemoryOutbox();
        
        cfg.UseInMemoryOutbox();
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