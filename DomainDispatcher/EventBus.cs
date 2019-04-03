using System;
using System.Collections.Generic;
using System.Text;

namespace DomainDispatcher
{
   
    using Abstractions;
    using Abstractions.Events;

    public class EventBus : IEventBus
    {
        private readonly IEventBusSubscriptionsManager _subsManager;

        public EventBus(IEventBusSubscriptionsManager subsManager )
        {
            _subsManager = subsManager;

            _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;

        }

        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            // do something
        }


        public void Publish(DomainEvent @event)
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe<T, TH>() where T : DomainEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();
            //DoInternalSubscription(eventName);
            _subsManager.AddSubscription<T, TH>();

             throw new NotImplementedException(); //return Disposable.Create(_subsManager.RemoveSubscription<T, TH>);
        }
    }
}
