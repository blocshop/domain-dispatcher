using System;
using System.Collections.Generic;
using System.Text;

namespace DomainDispatcher.Test
{
    using Abstractions.Events;

    public class SpecificDomainEvent : DomainEvent
    {
        public string MessageData { get; set; }
    }
}
