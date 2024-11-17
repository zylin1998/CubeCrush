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

            return WaitMovement(Map.SwapEmptyAll(clear));
        }

        public IObservable<int> ClearAndDrop(Vector2Int[] clear, (Vector2Int offset, int type)[] drops)
        {
            var subject = new Subject<int>();
            
            Clear(clear)
                .Subscribe((left) => { }, () =>
                {
                    Drop(drops)
                        .Subscribe(subject.OnNext, subject.OnError, subject.OnCompleted);
                });

            return subject;
        }

        public IObservable<int> Drop(IEnumerable<(Vector2Int offset, int type)> drops)
        {
            var list = drops.ToList();
            var max  = list.Max(d => d.offset.y);

            var subject = new IntReactiveProperty(list.Count);

            return WaitMovement(list.Select(drop =>
            {
                var offset  = Map[drop.offset];
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

                result = WaitMovement(new[] { offset1.CheckPosition(dropSpeed), LastSwap.CheckPosition(dropSpeed) });
                
                (LastSwap, Swapped) = (default, false);
            }

            if (!Swapped && distance != Vector2Int.zero)
            {
                Map.Swap(offset1.Offset, offset2.Offset);

                result = WaitMovement(new[] { offset1.CheckPosition(dropSpeed), offset2.CheckPosition(dropSpeed) });

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

        private IObservable<int> WaitMovement(IEnumerable<IObservable<Vector3>> movements) 
        {
            var list = movements.ToList();

            var subject = new IntReactiveProperty(list.Count);

            list.ForEach(movement => movement.Subscribe((posi) => { }, () => --subject.Value));

            return subject.TakeWhile(left => left > 0);
        }
    }
}