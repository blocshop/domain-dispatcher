namespace DomainDispatcher
{
    using System;
    using Abstractions;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainDispatcher(this IServiceCollection containerBuilder, 
                                                            Action<DispatchConfigurator> configAction = null)
        {
            configAction = configAction ?? DispatchConfigurator.DefaultSettings;
            
            containerBuilder.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            containerBuilder.AddSingleton<IEventBus>(serviceProvider =>
            {
                var bus = new EventBus(serviceProvider, serviceProvider.GetService<IEventBusSubscriptionsManager>());
                var configurator = new DispatchConfigurator(containerBuilder,  bus);
                configAction(configurator);
                return bus;
            });
            
            return containerBuilder;
        }
    }
}
