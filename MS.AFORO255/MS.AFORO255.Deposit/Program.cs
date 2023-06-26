using Aforo255.Cross.Discovery.Consul;
using Aforo255.Cross.Discovery.Fabio;
using Aforo255.Cross.Event.Src;
using Aforo255.Cross.Http.Src;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Deposit.Data;
using MS.AFORO255.Deposit.Messages.CommandHandlers;
using MS.AFORO255.Deposit.Messages.Commands;
using MS.AFORO255.Deposit.Persistences;
using MS.AFORO255.Deposit.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureAppConfiguration((host, builder) =>
{
    var c = builder.Build();
    builder.AddNacosConfiguration(c.GetSection("nacosConfig"));
});

ConfigureConfiguration(builder.Configuration);
ConfigureServices(builder.Services);

var app = builder.Build();
ConfigureMiddleware(app, app.Services);
ConfigureEndpoints(app, app.Services);
DbCreated.CreateDbIfNotExists(app);
app.UseConsul();
app.Run();

void ConfigureConfiguration(ConfigurationManager configuration)
{

}
void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    
    services.AddDbContext<ContextDatabase>(
        options =>
        {
            options.UseNpgsql(builder.Configuration["cn:postgres"]);
        });
    services.AddScoped<ITransactionService, TransactionService>();
    services.AddScoped<IAccountService, AccountService>();
    services.AddMediatR(typeof(Program).GetTypeInfo().Assembly);
    services.AddRabbitMQ();
    services.AddTransient<IRequestHandler<TransactionCreateCommand, bool>, TransactionCommandHandler>();
    services.AddTransient<IRequestHandler<NotificationCreateCommand, bool>, NotificationCommandHandler>();
    services.AddProxyHttp();
    builder.Services.AddConsul();
    builder.Services.AddFabio();

}
void ConfigureMiddleware(IApplicationBuilder app, IServiceProvider services)
{
    app.UseAuthorization();
}
void ConfigureEndpoints(IEndpointRouteBuilder app, IServiceProvider services)
{
    app.MapControllers();
}

