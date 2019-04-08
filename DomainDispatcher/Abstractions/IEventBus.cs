namespace DomainDispatcher.Abstractions
{
    using System;
    using System.Threading.Tasks;
    using Events;

    public interface IEventBus
    {

        Task PublishAsync<TEvent>(TEvent @event) 
            where TEvent : DomainEvent;


        IDisposable Subscribe<TEventHandler>() 
            where TEventHandler : IEventHandler<DomainEvent>;

        IDisposable Subscribe(Type eventHandlerType);
    }
}