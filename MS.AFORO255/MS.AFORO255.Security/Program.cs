using Aforo255.Cross.Discovery.Consul;
using Aforo255.Cross.Discovery.Fabio;
using Aforo255.Cross.Discovery.Mvc;
using Aforo255.Cross.Token.Src;
using Aforo255.Cross.Tracing.Src.Zipkin;
using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Security.Data;
using MS.AFORO255.Security.Persistences;
using MS.AFORO255.Security.Services;

var builder = WebApplication.CreateBuilder(args);

//Nacos
builder.Host.ConfigureAppConfiguration((host, builder) =>
{
    var c = builder.Build();
    builder.AddNacosConfiguration(c.GetSection("nacosConfig"));
});


builder.Services.AddControllers();
builder.Services.AddDbContext<ContextDatabase>(
    opt =>
    {
        opt.UseMySQL(builder.Configuration["cn:mysql"]);
    });
builder.Services.AddScoped<IAccessService, AccessService>();
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
