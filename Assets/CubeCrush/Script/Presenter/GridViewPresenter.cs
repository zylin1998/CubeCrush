using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Loyufei;
using Loyufei.DomainEvents;
using UniRx.Triggers;
using Zenject;

namespace CubeCrush
{
    public class GridViewPresenter : Presenter
    {
        public GridViewPresenter(GridView view, CubeGridQuery query, DomainEventService service) : base(service)
        {
            View  = view;
            Query = query;

            Init();
        }

        public GridView      View  { get; }
        public CubeGridQuery Query { get; }

        [Inject]
        public GridVerify Verify { get; }

        private CheckFilled _Check = new CheckFilled();

        private float _CheckDelay = 0.5f;

        private void Init() 
        {
            var offsets = View.MapLayout().ToArray();
            
            offsets.ForEach(ListenerAdapting);
        }

        protected override void RegisterEvents()
        {
            Register<LayoutGrid>(GridLayout);
            Register<ClearCube> (GridUpdate);
            Register<GameOver>  (GameOver);
            Register<StartGame> (StartGame);
        }

        private void GridLayout(LayoutGrid layout) 
        {
            View.RemoveCubes();

            var cubes = Query.InsertCubes;

            View.Drop(cubes);
        }

        private void GridUpdate(ClearCube clear) 
        {
            var isClear = clear.ShouldClear;

            if (View.Swapped) { View.EndSwap(clear.ShouldClear); }

            if (isClear)
            {
                var clears = Query.Clears.ToArray();
                var drops  = Query.InsertCubes.ToArray();

                View
                    .Clear(clears)
                    .Subscribe(
                    (left) => { },
                    ()  =>
                    {
                        View
                            .Drop(drops)
                            .Subscribe(
                                (left) => { },
                                ()  =>CheckFilled());
                    });
            }

            else { View.SetMask(false); }
        }

        private void GameOver(GameOver gameOver) 
        {
            View.SetMask(true);
        }

        private void StartGame(StartGame start)
        {
            View.SetMask(false);
        }

        private void CheckFilled() 
        {
            Observable
                .Timer(TimeSpan.FromSeconds(_CheckDelay))
                .Subscribe(t => { SettleEvents(_Check); });
        }

        private void ListenerAdapting(MapOffset offset) 
        {
            var selectable = offset.Selectable;
            var observable = default(IObservable<int>);

            selectable
                .OnDragAsObservable()
                .Subscribe(data =>
                {
                    var target = data.pointerEnter.GetComponent<MapOffset>();

                    //if (target.IsDefault() || target ==  View.LastSwap) { return; }

                    observable = View.Swap(offset, target) ?? observable;
                });

            selectable
                .OnEndDragAsObservable()
                .Subscribe(data => 
                {
                    if (!View.Swapped || View.LastSwap.IsDefault()) { return; }

                    View.SetMask(true);

                    observable.Subscribe(
                        (l) => {  },
                        ()  => SettleEvents(new SwapCube(offset.Offset, View.LastSwap.Offset)));
                });
        }
    }
}
