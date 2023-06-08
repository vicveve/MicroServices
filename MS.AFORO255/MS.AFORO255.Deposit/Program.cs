using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Deposit.Data;
using MS.AFORO255.Deposit.Persistences;
using MS.AFORO255.Deposit.Services;

var builder = WebApplication.CreateBuilder(args);
ConfigureConfiguration(builder.Configuration);
ConfigureServices(builder.Services);

var app = builder.Build();
ConfigureMiddleware(app, app.Services);
ConfigureEndpoints(app, app.Services);
DbCreated.CreateDbIfNotExists(app);
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
            options.UseNpgsql(builder.Configuration["postgres:cn"]);
        });
    services.AddScoped<ITransactionService, TransactionService>();
}
void ConfigureMiddleware(IApplicationBuilder app, IServiceProvider services)
{
    app.UseAuthorization();
}
void ConfigureEndpoints(IEndpointRouteBuilder app, IServiceProvider services)
{
    app.MapControllers();
}

