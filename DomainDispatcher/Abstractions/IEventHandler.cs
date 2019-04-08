namespace DomainDispatcher.Abstractions
{
    using System.Threading.Tasks;
    using Events;

    public interface IEventHandler<in TEvent> : IEventHandler
       where TEvent : DomainEvent
    {
        Task Handle(TEvent @event);
    }

    public interface IEventHandler
    {
    }
}