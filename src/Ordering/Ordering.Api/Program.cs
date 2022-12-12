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

var app = builder.Build();

app.Run();