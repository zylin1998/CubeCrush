using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using UnityEngine;

namespace Loyufei.DomainEvents
{
    public class DomainEventService
    {
        public DomainEventService(SignalBus signalBus, IEnumerable<IDomainEventBus> eventBuses) 
        {
            SignalBus  = signalBus;
            EventBuses = eventBuses.ToDictionary(bus => bus.Group);
        }

        public SignalBus                           SignalBus  { get; }
        public Dictionary<object, IDomainEventBus> EventBuses { get; }
    }

    public static class DomainEventServiceExtensions 
    {
        public static void Register<TEvent>(
            this DomainEventService self, 
                 Action<TEvent>     callBack, 
                 object             group = null)
            where TEvent : IDomainEvent
        {
            if (self.EventBuses.TryGetValue(group, out var bus))
            {
                bus.Register(callBack);
            }
        }

        public static void UnRegister<TEvent>(
            this DomainEventService    self,
                 IEventHandler<TEvent> handler, 
                 object                group = null)
            where TEvent : IDomainEvent
        {
            if (self.EventBuses.TryGetValue(group, out var bus))
            {
                bus.UnRegister(handler);
            }
        }

        public static void Post(this DomainEventService self, IDomainEvent e, object group = null)
        {
            self.SignalBus.TryFireId(group, e);
        }

        public static void Post(this DomainEventService self, IEnumerable<IDomainEvent> events, object group = null)
        {
            events.ForEach(e => self.Post(e, group));
        }

        public static void Post(this DomainEventService self, IAggregateRoot root, object group = null) 
        {
            root.ForEach(e => self.Post(e, group));
            
            root.ClearEvent();
        }
    }
}