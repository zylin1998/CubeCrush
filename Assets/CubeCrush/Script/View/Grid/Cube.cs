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

        private void Awake()
        {
            
        }

        public IObservable<long> Move(Vector2 to, float speed, float delay)
        {
            var moving = Observable
                .EveryFixedUpdate()
                .Delay(TimeSpan.FromSeconds(delay))
                .TakeWhile(t => Mathf.Abs(Vector3.Distance(transform.position, to)) > 0.01f);

            moving
                .Subscribe(t =>
                {
                    var toward = Vector2.MoveTowards(transform.position, to, speed * Time.fixedDeltaTime);

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