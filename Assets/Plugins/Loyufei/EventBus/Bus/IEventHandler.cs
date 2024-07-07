using System;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei.DomainEvents
{
    public interface IEventHandler 
    {
        public void Invoke(IDomainEvent eventdata);
    }

    public interface IEventHandler<TEvent> : IEventHandler where TEvent : IDomainEvent
    {
        public void Invoke(TEvent eventdata);

        void IEventHandler.Invoke(IDomainEvent eventdata) => Invoke(eventdata is TEvent data ? data : default);
    }

    public class EventHandler<TEvent> : IEventHandler<TEvent> where TEvent : IDomainEvent
    {
        public EventHandler(Action<TEvent> callBack)
        {
            _CallBack = callBack;
        }
        
        protected Action<TEvent> _CallBack { get; }

        public virtual void Invoke(TEvent eventData)
        {
            _CallBack?.Invoke(eventData);
        }
    }
}