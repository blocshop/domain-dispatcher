using System;
using System.Threading.Tasks;
using DomainDispatcher.Abstractions;

namespace DomainDispatcher.Test
{

    public interface ITestService
    {
        void Execute(SpecificDomainEvent @event);
    }

    public class SpecificDomainEventHandler : IEventHandler<SpecificDomainEvent>
    {

        private readonly ITestService _testService;

        public SpecificDomainEventHandler(ITestService testService)
        {
            _testService = testService;
            
        }

        public async Task Handle(SpecificDomainEvent @event)
        {
            var rnd = new Random();
            var timeout = rnd.Next(50, 250);
            await Task.Delay(timeout);
            _testService.Execute(@event);
        }
    }
}
