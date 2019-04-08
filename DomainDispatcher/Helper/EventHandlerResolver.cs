using System;
using System.Linq;
using DomainDispatcher.Abstractions;

namespace DomainDispatcher.Helper
{
    public static class EventHandlerResolver
    {
        public static HandlerDescriptor Describe(Type eventHandlerType)
        {
            if( !(typeof(IEventHandler).IsAssignableFrom(eventHandlerType)))
                throw new Exception($"can't register event handler [{eventHandlerType.FullName}]. Type must be inherited from {typeof(IEventHandler<>).FullName}");

            var handlerInterface = eventHandlerType.GetInterface(typeof(IEventHandler<>).Name);
            var domainEventType = handlerInterface.GenericTypeArguments.First();
            return new HandlerDescriptor
            {
                EvenName = domainEventType.Name,
                DomainEventType = domainEventType,
                HandlerType = eventHandlerType,
                HandlerInterface = handlerInterface
            };
        }
    }

    public class HandlerDescriptor
    {
        public string EvenName { get; set; }
        public Type DomainEventType { get; set; }
        public Type HandlerType { get; set; }
        public Type HandlerInterface { get; set; }
    }
}