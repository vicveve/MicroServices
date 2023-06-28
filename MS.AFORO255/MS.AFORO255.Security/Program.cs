using Aforo255.Cross.Discovery.Consul;
using Aforo255.Cross.Discovery.Fabio;
using Aforo255.Cross.Discovery.Mvc;
using Aforo255.Cross.Log.Src.Elastic;
using Aforo255.Cross.Metric.Metrics;
using Aforo255.Cross.Metric.Registry;
using Aforo255.Cross.Token.Src;
using Aforo255.Cross.Tracing.Src.Zipkin;
using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Security.Data;
using MS.AFORO255.Security.Persistences;
using MS.AFORO255.Security.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Nacos
builder.Host.ConfigureAppConfiguration((host, builder) =>
{
    var c = builder.Build();
    builder.AddNacosConfiguration(c.GetSection("nacosConfig"));
});
builder.WebHost.UseAppMetrics();
ExtensionsElastic.ConfigureLog(builder.Configuration);
builder.WebHost.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddDbContext<ContextDatabase>(
    opt =>
    {
        opt.UseMySQL(builder.Configuration["cn:mysql"]);
    });
builder.Services.AddScoped<IAccessService, AccessService>();
builder.Services.AddScoped<IMetricsRegistry, MetricsRegistry>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("jwt"));
builder.Services.AddConsul();
builder.Services.AddFabio();
builder.Services.AddJZipkin();
var app = builder.Build();
app.UseAuthorization();
app.MapControllers();
app.UseConsul();

DbCreated.CreateDbIfNotExists(app);
app.Run();
