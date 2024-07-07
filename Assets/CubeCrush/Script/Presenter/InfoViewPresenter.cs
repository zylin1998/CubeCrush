using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei;
using Loyufei.DomainEvents;
using Loyufei.ViewManagement;

namespace CubeCrush
{
    public class InfoViewPresenter : Presenter
    {
        public InfoViewPresenter(InfoView view, DomainEventService service) : base(service)
        {
            View = view;

            _OptionFunctions = new()
            {
                { 0, Start },
                { 1, Quit  },
            };

            Init();
        }

        public InfoView View { get; }

        private Dictionary<object, Action<IListenerAdapter>> _OptionFunctions;

        private StartGame _Start = new StartGame();

        private void Init() 
        {
            View.ForEach(adapter =>
            {
                adapter.AddListener(id => _OptionFunctions[id].Invoke(adapter));
            });
        }

        private void Start(IListenerAdapter adapter) 
        {
            SettleEvents(_Start);

            ((ButtonListener)adapter).Listener.interactable = false;
        }

        private void Quit(IListenerAdapter adapter) 
        {
            Application.Quit();
        }
    }
}