using System;
using System.Collections.Generic;
using System.Text;

namespace DomainDispatcher
{
    using Abstractions;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainDispatcher(this IServiceCollection containerBuilder)
        {
            containerBuilder.AddSingleton<IEventBus, EventBus>();
            containerBuilder.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            return containerBuilder;
        }
    }
}
