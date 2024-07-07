using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace CubeCrush
{
    public class GridVerify
    {
        [Inject]
        public CubeGrid Grid { get; }
        [Inject]
        public Map      Map  { get; }

        public IEnumerable<(Vector2Int offset, bool same)> Verified() 
        {
            var (x, y) = (0, 0);
            for (var i = 0; i < Declarations.Width * Declarations.Height; i++) 
            {
                var offset = new Vector2Int(x, y);
                
                yield return (offset, Equals(Grid.Get(x, y), Map[offset].Cube.Type));

                (x, y) = x == Declarations.Width - 1 ? (0, ++y) : (++x, y);
            }
        }

        public void ShowVerified() 
        {
            var verify  = Verified().ToArray();
            var correct = verify.All(v => v.same);
            var list    = string.Join("\n", verify.Select(v => string.Format("{0}: {1}", v.offset, v.same)));
            
            Debug.Log(string.Format("Result: {0}", correct) + "\n" + list);
        }
    }
}