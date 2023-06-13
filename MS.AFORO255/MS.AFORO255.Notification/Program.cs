using Aforo255.Cross.Event.Src.Bus;
using Aforo255.Cross.Event.Src;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Notification.Data;
using MS.AFORO255.Notification.Persistences;
using MS.AFORO255.Notification.Messages.Events;
using MS.AFORO255.Notification.Messages.EventHandlers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ContextDatabase>(
    opt =>
    {
        opt.UseMySQL(builder.Configuration["mariadb:cn"]);
    });


/*Start - RabbitMQ*/
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddRabbitMQ();

builder.Services.AddTransient<NotificacionEventHandler>();
builder.Services.AddTransient<IEventHandler<NotificationCreatedEvent>, NotificacionEventHandler>();
/*End - RabbitMQ*/


var app = builder.Build();
DbCreated.CreateDbIfNotExists(app);
ConfigureEventBus(app);
app.Run();

void ConfigureEventBus(IApplicationBuilder app)
{
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
    eventBus.Subscribe<NotificationCreatedEvent, NotificacionEventHandler>();
}