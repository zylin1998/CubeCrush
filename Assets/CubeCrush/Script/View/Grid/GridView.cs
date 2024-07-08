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

        public void Clear(Vector2Int[] clear)
        {
            clear.ForEach(offset => Map[offset].Clear());

            Map.SwapEmptyAll(clear);
        }

        public void ClearAndDrop(Vector2Int[] clear, (Vector2Int offset, int type)[] drops)
        {
            Clear(clear);

            Map.AwaitMoving(() => Drop(drops));
        }

        public void Drop((Vector2Int offset, int type)[] drops)
        {
            var max = drops.Max(d => d.offset.y);

            drops.Select(drop =>
            {
                var offset = Map[drop.offset];

                offset.Cube = CubePool.Spawn(drop.type, new(offset.Position.x, _Instantiate.position.y));

                Observable
                    .Timer(TimeSpan.FromSeconds((max - drop.offset.y) * 0.1f))
                    .Subscribe(t => offset.CheckPosition(Map.DropSpeed));

                return offset;
            }).ToArray();
        }

        public bool      Swapped  { get; private set; } = false;
        public MapOffset LastSwap { get; private set; }
        public MapOffset LastDrag { get; private set; }

        public void Swap(MapOffset offset1, MapOffset offset2)
        {
            if (!offset1 || !offset2) { return; }

            LastDrag = offset1;

            var distance = offset1.Offset - offset2.Offset;

            if (distance.x != 0 && distance.y != 0)     { return; }
            if (Mathf.Abs(distance.x + distance.y) > 1) { return; }
            
            if (Swapped && distance == Vector2Int.zero)
            {
                Map.Swap(offset1.Offset, LastSwap.Offset);
                
                offset1.CheckPosition(Map.DropSpeed);
                LastSwap.CheckPosition(Map.DropSpeed);

                LastSwap = default;

                Swapped  = false;
            }

            if (!Swapped && distance != Vector2Int.zero)
            {
                Map.Swap(offset1.Offset, offset2.Offset);

                offset1.CheckPosition(Map.DropSpeed);
                offset2.CheckPosition(Map.DropSpeed);

                LastSwap = offset2;

                Swapped = true;
            }
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
}