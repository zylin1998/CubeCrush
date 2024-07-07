using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei.DomainEvents
{
    public static class AggregateRootExtensions
    {
        public static void SettleEvents(this IAggregateRoot self, object identifier = null, params IDomainEvent[] domainEvents) 
        {
            self.AddEvents(domainEvents);

            self.DomainEventService.Post(self, identifier);
        }

        public static void SettleEvents(this IAggregateRoot self, IEnumerable<IDomainEvent> domainEvents, object identifier = null)
        {
            self.AddEvents(domainEvents);

            self.DomainEventService.Post(self, identifier);
        }
    }
}
