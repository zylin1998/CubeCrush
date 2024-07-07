using DG.Tweening;
using Loyufei;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.ViewManagement
{
    public enum EShowViewMode
    {
        Single = 0,
        Additive = 1,
    }

    public class ViewManager
    {
        public ViewManager() 
        {
            Views = new();
        }

        public Dictionary<object, IView> Views { get; }

        public IView Additive { get; private set; }
        public IView Current  { get; private set; }

        public void Register(object key, IView view)
        {
            Views.Add(key, view);
        }

        public bool Unregister(object key)
        {
            return Views.Remove(key);
        }

        public void Show(object key, Action onStart, Action onComplete, EShowViewMode viewMode = EShowViewMode.Single)
        {
            switch (viewMode)
            {
                case EShowViewMode.Single:
                    ShowSingle(key, onStart, onComplete); break;
                case EShowViewMode.Additive:
                    ShowAdditive(key, onStart, onComplete); break;
                default:
                    Debug.Log("Show View Mode Error"); break;
            }
        }

        public void ShowSingle(object key, Action onStart, Action onComplete)
        {
            if (Current.IsDefault())
            {
                Current = Show(key, onStart, onComplete);

                return;
            }

            Close(EShowViewMode.Single).OnComplete(() =>
            {
                Current?.To<Component>()?.gameObject.SetActive(false);

                Current = Show(key, onStart, onComplete);
            });
        }

        public void ShowAdditive(object key, Action onStart, Action onComplete)
        {
            Additive = Show(key, onStart, onComplete);
        }

        private IView Show(object key, Action onStart, Action onComplete)
        {
            Views.TryGetValue(key, out IView view);

            view?
                .Open()
                .OnStart(() =>
                {
                    view.To<Component>()?.gameObject.SetActive(true);
                    
                    onStart?.Invoke();
                })
                .OnComplete(() => onComplete?.Invoke());

            return view;
        }

        public Tween CloseCurrent() 
        {
            return Current?.Close().OnComplete(() =>
            {
                Current?.To<Component>()?.gameObject.SetActive(false);

                Current = null;
            });
        }

        public Tween CloseAdditive()
        {
            return Additive?.Close().OnComplete(() =>
            {
                Additive?.To<Component>()?.gameObject.SetActive(false);

                Additive = null;
            });
        }

        public Tween Close(EShowViewMode showMode)
        {
            switch(showMode) 
            {
                case EShowViewMode.Single:
                    return CloseCurrent();
                case EShowViewMode.Additive: 
                    return CloseAdditive();
            }

            return default;
        }
    }
}