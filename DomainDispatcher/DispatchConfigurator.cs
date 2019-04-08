namespace DomainDispatcher
{
    using System;
    using System.Linq;
    using Abstractions;
    using Abstractions.Events;
    using Microsoft.Extensions.DependencyInjection;

    public class DispatchConfigurator
    {
        private readonly IServiceCollection _serviceCollection;
        private readonly IEventBus _eventBus;

        public DispatchConfigurator(IServiceCollection serviceCollection, IEventBus eventBus)
        {
            _serviceCollection = serviceCollection;
            _eventBus = eventBus;
        }

        public DispatchConfigurator Subscribe<TEventHandler>() where TEventHandler : IEventHandler<DomainEvent>
        {
            _eventBus.Subscribe<TEventHandler>();
            return this;
        }
        
        public DispatchConfigurator SubscribeToAll()
        {
            var handlers = _serviceCollection
                                .Where(t => typeof(IEventHandler).IsAssignableFrom(t.ImplementationType))
                                .Select(t=> t.ImplementationType)
                                .Distinct()
                                .ToList();
            foreach (var handler in handlers)
            {
                _eventBus.Subscribe(handler);
            }
            

            return this;
        }

        public static Action<DispatchConfigurator> DefaultSettings => cfg => cfg.SubscribeToAll();
    }
}
