using System;
using System.Collections;
using System.Collections.Generic;
using Loyufei.DomainEvents;

namespace Loyufei.MVP
{
    public class Presenter : AggregateRoot
    {
        public Presenter(DomainEventService service) : base(service)
        {
            RegisterEvents();
        }

        public virtual object GroupId { get; }

        protected virtual void RegisterEvents() 
        {

        }

        protected void SettleEvents(params IDomainEvent[] events) 
        {
            this.SettleEvents(GroupId, events);
        }

        protected void Register<TDomainEvent>(Action<TDomainEvent> callBack) where TDomainEvent : IDomainEvent
        {
            DomainEventService.Register(callBack, GroupId);
        }
    }
}