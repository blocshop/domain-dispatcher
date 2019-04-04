namespace DomainDispatcher.Abstractions
{
    using System.Threading.Tasks;
    using Events;

    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
       where TIntegrationEvent : DomainEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IIntegrationEventHandler
    {
        
    }
}