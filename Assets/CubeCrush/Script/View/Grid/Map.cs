using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Zenject;
using Loyufei;

namespace CubeCrush
{
    public class Map : MonoBehaviour
    {
        [SerializeField]
        private GridLayoutGroup _LayoutGroup;
        [SerializeField]
        private float           _DropSpeed = 1000f;

        [Inject]
        protected MapOffset.Pool Pool { get; }

        private List<MapOffset> _Offsets = new();

        public float DropSpeed => _DropSpeed;

        public MapOffset this[Vector2Int offset] 
            => _Offsets.FirstOrDefault(o => Equals(o.Offset, offset));

        public IEnumerable<MapOffset> Layout() 
        {
            Pool.Content = transform;

            var (x, y) = (0, 0);
            for(var index = 0; index < Declarations.Width * Declarations.Height; ++index) 
            {
                _Offsets.Add(Pool.Spawn(new(x, y)));
                
                (x, y) = x == Declarations.Width - 1 ? (0, ++y) : (++x, y);
            }

            _LayoutGroup.CalculateLayoutInputHorizontal();
            _LayoutGroup.CalculateLayoutInputVertical();
            _LayoutGroup.SetLayoutHorizontal();
            _LayoutGroup.SetLayoutVertical();

            return _Offsets;
        }

        public void RemoveCubes() 
        {
            _Offsets.ForEach(offset => offset.Clear());
        }

        public bool IsEmpty(Vector2Int offset) 
        {
            return this[offset].Cube.IsDefault();
        }

        public void Swap(Vector2Int offset1, Vector2Int offset2)
        {
            var mapOffset1 = this[offset1];
            var mapOffset2 = this[offset2];

            var temp = mapOffset1.Cube;

            mapOffset1.Cube = mapOffset2.Cube;
            mapOffset2.Cube = temp;
        }

        public void SwapEmptyAll(Vector2Int[] clears)
        {
            clears.ForEach(SwapEmpty);

            _Offsets.ForEach(offset =>
            {
                if (offset.Dislocation) { offset.CheckPosition(DropSpeed); }
            });
        }

        public void AwaitMoving(Action onEnd) 
        {
            if (!_Offsets.Any(offset => offset.Dislocation))
            {
                onEnd?.Invoke();

                return;
            }

            Observable
                .EveryLateUpdate()
                .TakeWhile(t => _Offsets.Any(offset => offset.Dislocation))
                .Last()
                .Subscribe(t => onEnd?.Invoke());
        }

        private void SwapEmpty(Vector2Int offset)
        {
            if (!IsEmpty(offset)) { return; }

            var x = offset.x;

            for (var y = offset.y; y > 0; y--)
            {
                var offset1 = new Vector2Int(x, y);
                var offset2 = new Vector2Int(x, y - 1);
                
                Swap(offset1, offset2);
            }
        }
    }
}
