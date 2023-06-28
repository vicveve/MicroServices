using Aforo255.Cross.Discovery.Consul;
using Aforo255.Cross.Discovery.Fabio;
using Aforo255.Cross.Log.Src.Elastic;
using Aforo255.Cross.Metric.Metrics;
using MS.AFORO255.Account;
using MS.AFORO255.Account.Data;
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
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);
builder.Services.AddConsul();
builder.Services.AddFabio();

var app = builder.Build();
app.UseConsul();
startup.Configure(app, app.Lifetime);
startup.ConfigureEndpoints(app, app.Lifetime);
DbCreated.CreateDbIfNotExists(app);
app.Run();

