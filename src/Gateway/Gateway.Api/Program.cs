using System.Reflection;
using MassTransit;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((_, mqConfig) =>
    {
        mqConfig.Host("rabbitmq", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
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

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();