using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei;
using Loyufei.DomainEvents;
using Loyufei.ViewManagement.Event;

namespace Loyufei.ViewManagement
{
    public class ViewManagerPresenter
    {
        public ViewManagerPresenter(ViewManager manager, DomainEventService service, IEnumerable<IView> views)
        {
            _Manager = manager;

            service.Register<RegisterEvent>  (Register    , GroupId);
            service.Register<UnRegisterEvent>(UnRegister  , GroupId);
            service.Register<ShowEvent>      (Show        , GroupId);
            service.Register<CloseEvent>     (CloseCurrent, GroupId);

            views.ForEach(v => _Manager.Register(v.ViewId, v));
        }

        private ViewManager _Manager;

        public virtual object GroupId { get; }

        public void Register(RegisterEvent register) 
        {
            _Manager.Register(register.ViewId, register.View);
        }

        public void UnRegister(UnRegisterEvent register)
        {
            _Manager.Unregister(register.ViewId);
        }

        public void Show(ShowEvent show) 
        {
            _Manager.Show(show.ViewId, show.OnStart, show.OnComplete, show.ViewMode);
        }

        public void CloseCurrent(CloseEvent close) 
        {
            _Manager.Close(close.ShowMode);
        }
    }
}

namespace Loyufei.ViewManagement.Event 
{
    public class RegisterEvent : DomainEventBase
    {
        public RegisterEvent(object viewId, IView view) : base()
        {
            ViewId  = viewId;
            View    = view;
        }

        public object ViewId  { get; }
        public IView  View    { get; }
    }

    public class UnRegisterEvent : DomainEventBase
    {
        public UnRegisterEvent(object viewId) : base() 
        {
            ViewId  = viewId;
        }

        public object ViewId  { get; }
    }

    public class ShowEvent : DomainEventBase 
    {
        public ShowEvent(object viewId, EShowViewMode viewMode = EShowViewMode.Single) 
            : this(viewId, null, null, viewMode)
        {
            
        }

        public ShowEvent(object viewId, Action onStart, Action onComplete, EShowViewMode viewMode = EShowViewMode.Single) 
            : base() 
        {
            ViewId     = viewId;
            ViewMode   = viewMode;
            OnStart    = onStart;
            OnComplete = onComplete;
        }

        public object        ViewId     { get; }
        public EShowViewMode ViewMode   { get; }
        public Action        OnStart    { get; }
        public Action        OnComplete { get; }
    }

    public class CloseEvent : DomainEventBase
    {
        public CloseEvent() : this(EShowViewMode.Additive) 
        {
           
        }

        public CloseEvent(EShowViewMode showMode) : base()
        {
            ShowMode = showMode;
        }

        public EShowViewMode ShowMode { get; }
    }
}