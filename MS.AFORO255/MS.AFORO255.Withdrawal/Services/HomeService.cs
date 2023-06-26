using Aforo255.Cross.Event.Src.Bus;

namespace MS.AFORO255.Withdrawal.Services
{
    public class HomeService : IHomeService
    {

        private readonly IEventBus _eventBus;

        public HomeService(IEventBus eventBus)
        {
            
            _eventBus = eventBus;
        }
        public void Ping()
        {
            return;
        }
    }
}
