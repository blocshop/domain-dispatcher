using DomainDispatcher.Abstractions;
using DomainDispatcher.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DomainDispatcher.Test
{
    [TestClass]
    public class EventHandlerResolverTest
    {
        [TestMethod]
        public void ResolveHandlerData()
        {
            var descriptor = EventHandlerResolver.Describe(typeof(SpecificDomainEventHandler));
            Assert.IsNotNull(descriptor);
            Assert.AreEqual(descriptor.EvenName,nameof(SpecificDomainEvent));
            Assert.AreEqual(descriptor.DomainEventType,typeof(SpecificDomainEvent));
            Assert.AreEqual(descriptor.HandlerType,typeof(SpecificDomainEventHandler));
            Assert.AreEqual(descriptor.HandlerInterface,typeof(IEventHandler<SpecificDomainEvent>));
        }
    }
}
