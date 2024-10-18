using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;
using Loyufei;

namespace CubeCrush
{
    public class GridView : MonoBehaviour
    {
        [SerializeField]
        private Transform _Instantiate;
        [SerializeField]
        private Transform _Mask;

        [Inject]
        public Map Map { get; }
        [Inject]
        public Cube.Pool CubePool { get; }

        public IEnumerable<MapOffset> MapLayout()
        {
            return Map.Layout();
        }

        public void RemoveCubes() 
        {
            Map.RemoveCubes();
        }

        public IObservable<int> Clear(Vector2Int[] clear)
        {
            clear.ForEach(offset => Map[offset].Clear());

            return new MovementObservable(Map.SwapEmptyAll(clear));
        }

        public IObservable<int> ClearAndDrop(Vector2Int[] clear, (Vector2Int offset, int type)[] drops)
        {
            Clear(clear);

            return Drop(drops);
        }

        public IObservable<int> Drop(IEnumerable<(Vector2Int offset, int type)> drops)
        {
            var max = drops.Max(d => d.offset.y);

            return new MovementObservable(drops
                .Select(drop =>
                {
                    var offset = Map[drop.offset];
                    var dueTime = (max - drop.offset.y) * 0.1f;

                    offset.Cube = CubePool.Spawn(drop.type, new(offset.Position.x, _Instantiate.position.y));

                    return offset.CheckPosition(Map.DropSpeed, dueTime);
                }));
        }

        public bool      Swapped  { get; private set; } = false;
        public MapOffset LastSwap { get; private set; }
        public MapOffset LastDrag { get; private set; }

        public IObservable<int> Swap(MapOffset offset1, MapOffset offset2)
        {
            if (!offset1 || !offset2) { return default; }

            LastDrag = offset1;

            var distance = offset1.Offset - offset2.Offset;

            if (distance.x != 0 && distance.y != 0)     { return default; }
            if (Mathf.Abs(distance.x + distance.y) > 1) { return default; }

            var result    = default(IObservable<int>);
            var dropSpeed = Map.DropSpeed;

            if (Swapped && distance == Vector2Int.zero)
            {
                Map.Swap(offset1.Offset, LastSwap.Offset);

                result = new MovementObservable(offset1.CheckPosition(dropSpeed), LastSwap.CheckPosition(dropSpeed));
                
                (LastSwap, Swapped) = (default, false);
            }

            if (!Swapped && distance != Vector2Int.zero)
            {
                Map.Swap(offset1.Offset, offset2.Offset);

                result = new MovementObservable(offset1.CheckPosition(dropSpeed), offset2.CheckPosition(dropSpeed));

                (LastSwap, Swapped) = (offset2, true);
            }

            return result;
        }

        public void EndSwap(bool swapped) 
        {
            if (!swapped) 
            {
                Map.Swap(LastDrag.Offset, LastSwap.Offset);

                LastSwap.CheckPosition(Map.DropSpeed);
                LastDrag.CheckPosition(Map.DropSpeed);
            }

            LastSwap = default;
            LastDrag = default;

            Swapped  = false;
        }

        public void SetMask(bool mask) 
        {
            _Mask.gameObject.SetActive(mask);
        }
    }

    public class MovementObservable : IObservable<int>, IObserver<int> 
    {
        private class DisSubscribe : IDisposable
        {
            public DisSubscribe(List<IObserver<int>> observers, IObserver<int> target) 
            {
                _Observers = observers;
                _Target    = target;
            }

            private List<IObserver<int>> _Observers;
            private IObserver<int> _Target;

            public void Dispose()
            {
                _Observers.Remove(_Target);
            }
        }

        public MovementObservable(params IObservable<long>[] observables) : this((IEnumerable<IObservable<long>>)observables) 
        {

        }

        public MovementObservable(IEnumerable<IObservable<long>> observables) 
        {
            _Observables = observables.ToList();
            _Observers   = new();

            var index = 0;
            _Observables.ForEach(o =>
            {
                o
                .LastOrDefault()
                .Subscribe(
                    (l) => { },
                    () => OnNext(index));
                
                index++;
            });
        }

        private List<IObservable<long>> _Observables;
        private List<IObserver<int>>    _Observers;

        private int _Finished = 0;

        public void OnNext(int index) 
        {
            var count = _Observables.Count;
            
            _Finished++;

            _Observers.ForEach(o => o.OnNext(count - _Finished));
            
            if (_Finished == _Observables.Count) { OnCompleted(); }

            if (_Finished <= 0 || _Finished > _Observables.Count) { OnError(new ArgumentOutOfRangeException()); }
        }

        public void OnCompleted()
        {
            _Observers.ForEach(o => o.OnCompleted());
        }

        public void OnError(Exception error)
        {
            _Observers.ForEach(o => o.OnError(error));
        }

        public IDisposable Subscribe(IObserver<int> observer)
        {
            _Observers.Add(observer);
            
            var count = _Observables.Count;

            if (count == 0 || _Finished == count) { observer.OnCompleted(); }

            return new DisSubscribe(_Observers, observer);
        }
    }
}