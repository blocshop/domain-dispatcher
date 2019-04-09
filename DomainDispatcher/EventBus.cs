using DomainDispatcher.Helper;

namespace DomainDispatcher
{
    using System;
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

        public async Task PublishAsync<TEvent>(TEvent @event)
            where TEvent: DomainEvent
        {

            if (_subsManager.HasSubscriptionsForEvent<TEvent>())
            {
                var scopeFactory = _provider.GetRequiredService<IServiceScopeFactory>();

                if (scopeFactory == null)
                {
                    await InvokeMessageHandlers(@event, _provider);
                }
                else
                {
                    using (var scope = scopeFactory.CreateScope())
                    {
                        await InvokeMessageHandlers( @event, scope.ServiceProvider);
                    }
                }
            }
        }

        private async Task InvokeMessageHandlers<TEvent>(TEvent @event, IServiceProvider provider)
            where TEvent: DomainEvent
        {
            var subscriptions = _subsManager.GetHandlersForEvent<TEvent>();
            foreach (var subscription in subscriptions)
            {
                var handler = (IEventHandler<TEvent>)provider.GetService(subscription.HandlerType);
                await handler.Handle(@event);
            }
        }

       
        public IDisposable Subscribe<TEventHandler>() where TEventHandler : IEventHandler<DomainEvent>
        {
            return Subscribe(typeof(TEventHandler));
        }

        public IDisposable Subscribe(Type eventHandlerType) 
        {
            _subsManager.AddSubscription(eventHandlerType);
            return Disposable.Create(() => _subsManager.RemoveSubscription(eventHandlerType) );
        }
    }
}
