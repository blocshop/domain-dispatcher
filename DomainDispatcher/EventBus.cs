using System;
using System.Collections.Generic;
using System.Text;

namespace DomainDispatcher
{
    using System.Reactive.Disposables;
    using System.Threading.Tasks;
    using Abstractions;
    using Abstractions.Events;
    using Microsoft.Extensions.DependencyInjection;

    public class EventBus : IEventBus
    {
        private readonly IServiceProvider _provider;
        private readonly IEventBusSubscriptionsManager _subsManager;

        public EventBus(IServiceProvider provider, IEventBusSubscriptionsManager subsManager )
        {
            _provider = provider;
            _subsManager = subsManager;
        }

        public async Task PublishAsync(DomainEvent @event)
        {
            var eventName = @event.GetType().Name;

            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                var scopeFactory = _provider.GetRequiredService<IServiceScopeFactory>();

                if (scopeFactory == null)
                {
                    await InvokeMessageHandlers(eventName, @event, _provider);
                }
                else
                {
                    using (var scope = scopeFactory.CreateScope())
                    {
                        await InvokeMessageHandlers(eventName, @event, scope.ServiceProvider);
                    }
                }
            }
        }

        private async Task InvokeMessageHandlers(string eventName, DomainEvent @event, IServiceProvider provider)
        {
            var subscriptions = _subsManager.GetHandlersForEvent(eventName);
            foreach (var subscription in subscriptions)
            {
                var eventType = _subsManager.GetEventTypeByName(eventName);
                var handler = provider.GetService(subscription.HandlerType);

                var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
            }
        }

        public IDisposable Subscribe<T, TH>() where T : DomainEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();
            //DoInternalSubscription(eventName);
            _subsManager.AddSubscription<T, TH>();
            return Disposable.Create(_subsManager.RemoveSubscription<T, TH>);
        }
    }
}
