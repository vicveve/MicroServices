using Aforo255.Cross.Token.Src;
using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Security.Data;
using MS.AFORO255.Security.Persistences;
using MS.AFORO255.Security.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<ContextDatabase>(
    opt =>
    {
        opt.UseMySQL(builder.Configuration["mysql:cn"]);
    });
builder.Services.AddScoped<IAccessService, AccessService>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("jwt"));

var app = builder.Build();
app.UseAuthorization();
app.MapControllers();
DbCreated.CreateDbIfNotExists(app);
app.Run();
