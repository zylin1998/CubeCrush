using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;

namespace Loyufei.ViewManagement
{
    public interface IView 
    {
        public object ViewId { get; }

        public Tween Open();

        public Tween Close();
    }

    public abstract class ViewMono : MonoBehaviour, IView, IEnumerable<IListenerAdapter>
    {
        [SerializeField]
        private bool _InitActive;

        protected virtual void Awake() 
        {
            gameObject.SetActive(_InitActive);
        }

        public abstract object ViewId { get; }

        public abstract Tween Open();

        public abstract Tween Close();

        public abstract IEnumerator<IListenerAdapter> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public abstract class View : IView
    {
        public View()
        {

        }

        public abstract object ViewId { get; }

        public abstract Tween Open();

        public abstract Tween Close();
    }
}