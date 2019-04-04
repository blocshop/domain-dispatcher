using System;
using System.Collections.Generic;
using System.Text;

namespace DomainDispatcher
{
    using Abstractions;
    using Abstractions.Events;
    using Microsoft.Extensions.DependencyInjection;

    public class DispatchConfigurator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventBus _eventBus;

        public DispatchConfigurator(IServiceProvider serviceProvider, IEventBus eventBus)
        {
            _serviceProvider = serviceProvider;
            _eventBus = eventBus;
        }

        

        public DispatchConfigurator Subscribe<T, TH>() where T : DomainEvent where TH : IIntegrationEventHandler<T>
        {
            _eventBus.Subscribe<T, TH>();
            return this;
        }
    }
}
