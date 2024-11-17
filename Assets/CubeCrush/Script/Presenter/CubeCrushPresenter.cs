using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.DomainEvents;

namespace CubeCrush
{
    public class CubeCrushPresenter : Presenter
    {
        public CubeCrushPresenter(CubeCrushModel model, DomainEventService service) : base(service)
        {
            Model = model;
        }

        public CubeCrushModel Model { get; }

        private LayoutGrid _Layout = new LayoutGrid();

        protected override void RegisterEvents()
        {
            Register<StartGame>  (Start);
            Register<SwapCube>   (Swap);
            Register<CheckFilled>(Check);
        }

        private void Start(StartGame start) 
        {
            Model.Start();
            
            SettleEvents(_Layout);
        }

        private void Swap(SwapCube swap) 
        {
            bool swapped = Model.Swap(swap.Offset1, swap.Offset2);

            if (swapped) { Model.FillEmpty(); }
            
            SettleEvents(new ClearCube(swapped));
        }

        private void Check(CheckFilled check) 
        {
            var cleared = Model.CheckFilled();
            var clear   = new ClearCube(cleared);

            if (!cleared)
            {
                var gameOver = Model.GameOver();
                var events   = new List<IDomainEvent>() { clear };
                
                if (gameOver) 
                {
                    events.Add(new GameOver());
                }

                SettleEvents(events.ToArray());

                return;
            }
            
            Model.FillEmpty();

            SettleEvents(clear);
        }
    }

    #region Input Event

    public class StartGame : DomainEventBase 
    {

    }

    public class SwapCube : DomainEventBase 
    {
        public SwapCube(Vector2Int offset1, Vector2Int offset2)
        {
            Offset1 = offset1;
            Offset2 = offset2;
        }

        public Vector2Int Offset1 { get; }
        public Vector2Int Offset2 { get; }
    }

    public class CheckFilled : DomainEventBase 
    {

    }

    #endregion

    #region OutputEvent

    public class LayoutGrid : DomainEventBase 
    {

    }

    public class ClearCube : DomainEventBase 
    {
        public ClearCube(bool shouldClear)
        {
            ShouldClear = shouldClear;
        }

        public bool ShouldClear { get; }
    }

    public class GameOver : DomainEventBase 
    {

    }

    #endregion
}
