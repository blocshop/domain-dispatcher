namespace DomainDispatcher.Abstractions
{
    using System;
    using Events;

    public interface IEventBus
    {
        void Publish(DomainEvent @event);

        IDisposable Subscribe<T, TH>()
            where T : DomainEvent
            where TH : IIntegrationEventHandler<T>;
    }
}