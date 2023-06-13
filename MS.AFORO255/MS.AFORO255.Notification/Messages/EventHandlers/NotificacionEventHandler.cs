using Aforo255.Cross.Event.Src.Bus;
using MS.AFORO255.Notification.Messages.Events;
using MS.AFORO255.Notification.Models;
using MS.AFORO255.Notification.Persistences;

namespace MS.AFORO255.Notification.Messages.EventHandlers
{
    public class NotificacionEventHandler : IEventHandler<NotificationCreatedEvent>
    {
        private readonly ContextDatabase _historyService;

        public NotificacionEventHandler(ContextDatabase historyService) 
        {
            _historyService = historyService;
        } 

        public Task Handle(NotificationCreatedEvent @event)
        {
            var not = new SendMailModel()
            {
                Type = @event.Type,
                AccountId = @event.AccountId,
                Address = @event.Address,
                Message = @event.MessageBody,
                SendDate = DateTime.Now.ToString()
            };

            _historyService.SendMail.Add(not);
            _historyService.SaveChanges();
            

            return Task.CompletedTask;
        }
    }
}
