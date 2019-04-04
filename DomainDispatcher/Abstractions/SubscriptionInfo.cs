namespace DomainDispatcher.Abstractions
{
    using System;

    public class SubscriptionInfo
    {
        public Type HandlerType { get; }

        private SubscriptionInfo(Type handlerType)
        {
            HandlerType = handlerType;
        }

        public static SubscriptionInfo New(Type handlerType)
        {
            return new SubscriptionInfo(handlerType);
        }
    }
}