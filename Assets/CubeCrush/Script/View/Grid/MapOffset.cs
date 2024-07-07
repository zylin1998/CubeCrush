using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Loyufei;

namespace CubeCrush
{
    public class MapOffset : MonoBehaviour
    {
        public class Pool : MemoryPool<Vector2Int, MapOffset> 
        {
            public Pool() 
            {
                DespawnRoot = new GameObject("MapOffsetPool").transform;
            }

            public Transform Content     { get; set; }
            public Transform DespawnRoot { get; }

            protected override void Reinitialize(Vector2Int offset, MapOffset mapOffset)
            {
                mapOffset.Offset = offset;

                mapOffset.transform.SetParent(Content, true);
            }

            protected override void OnDespawned(MapOffset mapOffset)
            {
                mapOffset.transform.SetParent(DespawnRoot);
            }
        }

        [SerializeField]
        private Selectable _Selectable;
        [SerializeField]
        private Vector2Int _Offset;

        private Cube _Cube;

        public Selectable Selectable => _Selectable;
        public Vector2Int Offset     { get => _Offset; set => _Offset = value; }
        public Cube       Cube       
        {
            get => _Cube; 
            
            set 
            {
                _Cube = value;

                _Cube?.transform.SetParent(transform);
            } 
        }

        public Vector2    Position => transform.position;

        public bool Dislocation => !Cube.IsDefault() && Vector2.Distance(Cube.transform.position, Position) != 0;

        public void Clear()
        {
            Cube.Recycle();

            Cube = default;
        }

        public IObservable<long> CheckPosition(float speed) 
        {
            if (Cube.IsDefault() || Vector2.Distance(Cube.transform.position, Position) <= 0) { return default; }

            return Cube.Move(Position, speed);
        }
    }
}