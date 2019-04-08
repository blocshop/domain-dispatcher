using System;
using System.Collections.Generic;
using DomainDispatcher.Abstractions.Events;

namespace DomainDispatcher.Abstractions
{
    public interface IEventBusSubscriptionsManager
    {
        event EventHandler<Type> OnSubscriptionReleased;

        bool HasSubscriptionsForEvent<T>() where T : DomainEvent;
        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : DomainEvent;

        void AddSubscription<THandler>()
            where THandler : IEventHandler<DomainEvent>;

        void AddSubscription(Type eventHandlerType);
        
        void RemoveSubscription(Type eventHandler);
        void RemoveSubscription<TEventHandler>()
            where TEventHandler : IEventHandler<DomainEvent>;
    }
}