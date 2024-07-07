using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Zenject;

namespace CubeCrush
{
    public class Cube : MonoBehaviour
    {
        public class Pool : MemoryPool<int, Vector2, Cube> 
        {
            public Pool() : base()
            {
                DespawnRoot = new GameObject("CubeRoot").transform;
            }

            public Transform DespawnRoot { get; }

            protected override void Reinitialize(int type, Vector2 position, Cube cube)
            {
                cube.Type = type;
                
                cube.transform.SetPositionAndRotation(position, Quaternion.identity);
                
                cube.gameObject.SetActive(true);
            }

            protected override void OnDespawned(Cube cube)
            {
                cube.transform.SetParent(DespawnRoot);

                cube.gameObject.SetActive(false);
            }
        }

        [SerializeField]
        private Image _Image;

        [Inject]
        private Cube.Pool _Pool;

        private int _Type;

        public int Type
        {
            get => _Type;

            set
            {
                _Type = value;

                _Image.color = Declarations.TypeColor[Type];
            }
        }

        public IObservable<long> Move(Vector2 to, float speed)
        {
            var delta = speed * Time.fixedDeltaTime;

            var moving = Observable
                .EveryFixedUpdate()
                .TakeWhile(t => Mathf.Abs(Vector2.Distance(transform.position, to)) > 0);
            
            moving
                .Subscribe(t =>
                {
                    var toward = Vector2.MoveTowards(transform.position, to, delta);

                    transform.SetPositionAndRotation(toward, Quaternion.identity);
                });

            return moving;
        }

        public void Recycle() 
        {
            _Pool.Despawn(this);
        }
    }
}