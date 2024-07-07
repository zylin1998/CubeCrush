using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using UnityEngine;

namespace Loyufei.DomainEvents
{
    public class DomainEventBus : IDomainEventBus
    {
        [Inject]
        public DomainEventBus(SignalBus signalBus, object group = null)
        {
            SignalBus = signalBus;
            Group     = group;
            
            SignalBus.SubscribeId<IDomainEvent>(Group, Post);
            
            _Mapping = new EventHandlerMapping();
        }

        private EventHandlerMapping _Mapping;

        public object    Group     { get; }
        public SignalBus SignalBus { get; }

        public Dictionary<Type, List<IEventHandler>> Mapping => _Mapping;

        public void Register<TEvent>(IEventHandler<TEvent> eventHandler, bool priority = false)
            where TEvent : IDomainEvent
        {
            var eventType   = typeof(TEvent);

            var handlers = _Mapping.GetorAdd(eventType, () => new());

            if (priority) { handlers.Insert(0, eventHandler); }

            else { handlers.Add(eventHandler); }
        }

        public bool UnRegister<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : IDomainEvent
        {
            var eventType = typeof(TEvent);
            var handlers  = _Mapping.GetorReturn(eventType, () => new());

            return handlers.Remove(eventHandler);
        }

        public void PostAll(IAggregateRoot trigger, object identifier = null)
        {
            foreach (var e in trigger)
            {
                SignalBus.TryFireId(identifier, e);
            }

            trigger.ClearEvent();
        }

        private void Post(IDomainEvent eventData)
        {
            var actions = _Mapping.GetorAdd(eventData.GetType(), () => new());
            
            actions.ForEach(action => action.Invoke(eventData));
        }

        internal class EventHandlerMapping : Dictionary<Type, List<IEventHandler>> 
        {

        }
    }
}