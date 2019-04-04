using System;
using System.Collections.Generic;
using System.Text;

namespace DomainDispatcher
{
    using Abstractions;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainDispatcher(this IServiceCollection containerBuilder, Action<DispatchConfigurator> configAction)
        {
            containerBuilder.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            containerBuilder.AddSingleton<IEventBus>(serviceProvider =>
            {
                var bus = new EventBus(serviceProvider, serviceProvider.GetService<IEventBusSubscriptionsManager>());
                var configurator = new DispatchConfigurator(serviceProvider, bus);
                configAction(configurator);
                return bus;
            });
            
            return containerBuilder;
        }
    }
}
