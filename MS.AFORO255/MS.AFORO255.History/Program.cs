using MS.AFORO255.History.Features.Services;
using MS.AFORO255.History.Persistences.Settings;
using MS.AFORO255.History.Persistences;
using Carter;
using MediatR;
using Aforo255.Cross.Event.Src;
using MS.AFORO255.History.Messages.EventHandlers;
using MS.AFORO255.History.Messages.Events;
using Aforo255.Cross.Event.Src.Bus;
using Aforo255.Cross.Discovery.Consul;
using Aforo255.Cross.Discovery.Fabio;
using Aforo255.Cross.Cache.Src;
using Aforo255.Cross.Tracing.Src.Zipkin;
using Aforo255.Cross.Metric.Registry;
using Aforo255.Cross.Metric.Metrics;
using Aforo255.Cross.Log.Src.Elastic;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Host.ConfigureAppConfiguration((host, builder) =>
{
    var c = builder.Build();
    builder.AddNacosConfiguration(c.GetSection("nacosConfig"));
});
builder.WebHost.UseAppMetrics();
ExtensionsElastic.ConfigureLog(builder.Configuration);
builder.WebHost.UseSerilog();
// Add services to the container.
builder.Services.AddCarter();
builder.Services.Configure<Mongosettings>(opt =>
{
    opt.Connection = builder.Configuration.GetSection("cn:mongo").Value;
    opt.DatabaseName = builder.Configuration.GetSection("mongo:database").Value;
});
builder.Services.AddScoped<IHistoryService, HistoryService>();
builder.Services.AddScoped<IMongoBookDBContext, MongoBookDBContext>();

/*Start - RabbitMQ*/
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddRabbitMQ();

builder.Services.AddTransient<TransactionEventHandler>();
builder.Services.AddTransient<IEventHandler<TransactionCreatedEvent>, TransactionEventHandler>();
/*End - RabbitMQ*/

builder.Services.AddConsul();
builder.Services.AddFabio();
builder.Services.AddJZipkin();
//REDIS
builder.Services.AddRedis();
builder.Services.AddSingleton<IExtensionCache, ExtensionCache>();

//Metricas
builder.Services.AddTransient<IMetricsRegistry, MetricsRegistry>();
var app = builder.Build();
// Configure the HTTP request pipeline.
app.MapCarter();
ConfigureEventBus(app);
app.UseConsul();
app.Run();


void ConfigureEventBus(IApplicationBuilder app)
{
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
    eventBus.Subscribe<TransactionCreatedEvent, TransactionEventHandler>();
}
