using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainDispatcher.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DomainDispatcher.Test
{
    [TestClass]
    public class IntegrationTest
    {
        [TestMethod]
        public async Task PublishEvent()
        {
            var sutProvider = new SUTProvider();
            var eventBus = sutProvider
                                    .SetupIoC(collection => { collection.AddTransient<SpecificDomainEventHandler>(); })
                                    .Build();
            
            await eventBus.PublishAsync(new SpecificDomainEvent{MessageData = "msg1"});
            
            Assert.AreEqual(sutProvider.EventCalls.Count,1);
            Assert.AreEqual(sutProvider.EventCalls[0].MessageData,"msg1");
        }
    }


    public class SUTProvider
    {
        private readonly ServiceCollection _serviceCollection = new ServiceCollection();
        private ServiceProvider _serviceProvider;
        private readonly Mock<ITestService> _testServiceMoq;
        
        public readonly List<SpecificDomainEvent> EventCalls  = new List<SpecificDomainEvent>(); 

        public SUTProvider()
        {
            _testServiceMoq = new Mock<ITestService>();
            _testServiceMoq
                .Setup(service => service.Execute(It.IsAny<SpecificDomainEvent>()))
                .Callback((SpecificDomainEvent evt) => EventCalls.Add(evt));
        }


        public SUTProvider SetupIoC(Action<IServiceCollection> setupAction)
        {
            setupAction(_serviceCollection);
            _serviceCollection.AddSingleton(_testServiceMoq.Object);
            _serviceCollection.AddDomainDispatcher();
            return this;
        }

        public IEventBus Build()
        {
            _serviceProvider = _serviceCollection.BuildServiceProvider();
            return _serviceProvider.GetRequiredService<IEventBus>();
        }
    }
}