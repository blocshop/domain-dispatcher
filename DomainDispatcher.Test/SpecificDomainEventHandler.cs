using System;
using System.Collections.Generic;
using System.Text;

namespace DomainDispatcher.Test
{
    using System.Threading.Tasks;
    using Abstractions;

    public class SpecificDomainEventHandler : IIntegrationEventHandler<SpecificDomainEvent>
    {
        public Task Handle(SpecificDomainEvent @event)
        {
            Console.WriteLine("SpecificDomainEventHandler");
            Console.WriteLine("Press any key to continue..");
            Console.ReadLine();
            return Task.FromResult(true);
        }
    }
}
