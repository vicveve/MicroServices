using MS.AFORO255.History.Features.Services;
using MS.AFORO255.History.Persistences.Settings;
using MS.AFORO255.History.Persistences;
using Carter;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCarter();
builder.Services.Configure<Mongosettings>(opt =>
{
    opt.Connection = builder.Configuration.GetSection("mongo:cn").Value;
    opt.DatabaseName = builder.Configuration.GetSection("mongo:database").Value;
});
builder.Services.AddScoped<IHistoryService, HistoryService>();
builder.Services.AddScoped<IMongoBookDBContext, MongoBookDBContext>();

var app = builder.Build();
app.MapCarter();
app.Run();
