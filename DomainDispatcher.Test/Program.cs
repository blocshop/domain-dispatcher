using System;

namespace DomainDispatcher.Test
{
    using Abstractions;
    using Microsoft.Extensions.DependencyInjection;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var serviceCollection = new ServiceCollection();

                serviceCollection.AddTransient<SpecificDomainEventHandler>();
                serviceCollection.AddTransient<IIntegrationEventHandler<SpecificDomainEvent>, SpecificDomainEventHandler>();

                serviceCollection.AddDomainDispatcher((config) =>
                {
                    config.Subscribe<SpecificDomainEvent, SpecificDomainEventHandler>();
                });

                var provider = serviceCollection.BuildServiceProvider();

                var bus = provider.GetRequiredService<IEventBus>();

                bus.PublishAsync(new SpecificDomainEvent());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

    }
}
