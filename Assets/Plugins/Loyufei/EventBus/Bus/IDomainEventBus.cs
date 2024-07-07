using System;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei.DomainEvents 
{
    public interface IDomainEventBus
    {
        public object Group { get; }

        public void Register<TEvent>(IEventHandler<TEvent> eventHandler, bool priority = false) 
            where TEvent : IDomainEvent;

        public bool UnRegister<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : IDomainEvent;

        public void PostAll(IAggregateRoot trigger, object Identifier = null);
    }

    public static class DomainEventBusExtensions 
    {
        public static void Register<TEvent>(
            this IDomainEventBus self, 
                 Action<TEvent>  eventAction,
                 bool            priority = false)
            where TEvent : IDomainEvent
        {
            self.Register(new EventHandler<TEvent>(eventAction), priority);
        }
    }
}