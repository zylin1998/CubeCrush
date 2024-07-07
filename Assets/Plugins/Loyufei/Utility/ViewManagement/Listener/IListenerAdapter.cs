using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Loyufei.ViewManagement
{
    public class MonoListenerAdapter<TListener>
        : MonoBehaviour, IListenerAdapter<TListener> where TListener : Selectable
    {
        [SerializeField]
        private int       _Id;
        [SerializeField]
        private TListener _Listener;

        public int Id { get => _Id; set => _Id = value; }

        public TListener Listener => _Listener;

        public virtual void AddListener(Action<object> callBack)
        {
            
        }
    }

    public interface IListenerAdapter<TListener> : IListenerAdapter 
    {
        TListener Listener { get; }
    }

    public interface IListenerAdapter 
    {
        public int Id { get; }

        public void AddListener(Action<object> callBack);
    }
}