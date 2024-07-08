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
                { 0, Restart },
                { 1, Start   },
                { 2, Quit    },
            };

            Init();
        }

        public InfoView View { get; }

        private Dictionary<object, Action<IListenerAdapter>> _OptionFunctions;

        private StartGame _Start = new StartGame();

        private List<IListenerAdapter> _Listeners;

        protected override void RegisterEvents()
        {
            Register<GameOver>(GameOver);
        }

        private void Init() 
        {
            _Listeners = View.ToList();

            _Listeners.ForEach(adapter =>
            {
                adapter.AddListener(id => _OptionFunctions[id].Invoke(adapter));
            });

            _Listeners.FirstOrDefault(l => l.Id == 0).To<ButtonListener>().Listener.interactable = false;
        }

        private void Restart(IListenerAdapter adapter) 
        {
            SettleEvents(_Start);
        }

        private void Start(IListenerAdapter adapter) 
        {
            SettleEvents(_Start);

            ((ButtonListener)adapter).Listener.interactable = false;

            _Listeners.FirstOrDefault(l => l.Id == 0).To<ButtonListener>().Listener.interactable = true;
        }

        private void Quit(IListenerAdapter adapter) 
        {
            Application.Quit();
        }

        private void GameOver(GameOver gameOver) 
        {
            _Listeners.FirstOrDefault(l => l.Id == 0).To<ButtonListener>().Listener.interactable = true;
        }
    }
}