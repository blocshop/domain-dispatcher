namespace DomainDispatcher
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;
    using Abstractions.Events;
    using Helper;

    public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        private readonly Dictionary<Type, List<SubscriptionInfo>> _handlers;
        public event EventHandler<Type> OnSubscriptionReleased;

        public InMemoryEventBusSubscriptionsManager()
        {
            _handlers = new Dictionary<Type, List<SubscriptionInfo>>();
        }

        public void AddSubscription<THandler>()
            where THandler : IEventHandler<DomainEvent>
        {
            AddSubscription(typeof(THandler));
        }
        public void AddSubscription(Type eventHandlerType)
        {
            var handlerDescriptor = EventHandlerResolver.Describe(eventHandlerType);
            AddSubscriptionInternal(handlerDescriptor.HandlerType, handlerDescriptor.DomainEventType);
        }

        private void AddSubscriptionInternal(Type handlerType, Type eventType)
        {
            if (!HasSubscriptionsForEvent(eventType))
            {
                _handlers.Add(eventType, new List<SubscriptionInfo>());
            }

            if (_handlers[eventType].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already registered for '{eventType}'", nameof(handlerType));
            }

            _handlers[eventType].Add(SubscriptionInfo.New(handlerType));

        }
        public void RemoveSubscription<TEventHandler>()
            where TEventHandler : IEventHandler<DomainEvent>
        {
            RemoveSubscription(typeof(TEventHandler));
        }        

        public void RemoveSubscription(Type eventHandler)
        {
            var descriptor = EventHandlerResolver.Describe(eventHandler);
            var handlerToRemove = FindSubscriptionToRemove(descriptor.DomainEventType, descriptor.HandlerType);
            RemoveHandlerInternal(descriptor.DomainEventType, handlerToRemove);
        }


        private void RemoveHandlerInternal(Type domainEventType, SubscriptionInfo subscription)
        {
            if (subscription != null)
            {
                var subscriptions = _handlers[domainEventType];
                subscriptions.Remove(subscription);
                if (!subscriptions.Any())
                {
                    _handlers.Remove(domainEventType);
                    RaiseOnSubscriptionReleased( domainEventType);
                }
            }
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : DomainEvent
        {
            _handlers.TryGetValue(typeof(T), out var subscriptions);
            return subscriptions ?? new List<SubscriptionInfo>();
        }
        private void RaiseOnSubscriptionReleased(Type domainEventType)
        {
            var handler = OnSubscriptionReleased;
            handler?.Invoke(this, domainEventType);
        }

        private SubscriptionInfo FindSubscriptionToRemove(Type eventType, Type handlerType)
        {
            if (!HasSubscriptionsForEvent(eventType))
            {
                return null;
            }

            return _handlers[eventType].SingleOrDefault(s => s.HandlerType == handlerType);
        }

        public bool HasSubscriptionsForEvent<T>() where T : DomainEvent
        {
            return HasSubscriptionsForEvent(typeof(T));
        }

        private bool HasSubscriptionsForEvent(Type eventType) => _handlers.ContainsKey(eventType);
    }
}
