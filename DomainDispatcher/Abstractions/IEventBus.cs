namespace DomainDispatcher.Abstractions
{
    using System;
    using System.Threading.Tasks;
    using Events;

    public interface IEventBus
    {
        Task PublishAsync(DomainEvent @event);

        IDisposable Subscribe<T, TH>()
            where T : DomainEvent
            where TH : IIntegrationEventHandler<T>;
    }
}