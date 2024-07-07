using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeCrush
{
    public class CubeGridQuery
    {
        public IEnumerable<(Vector2Int offset, int value)> InsertCubes { get; set; }

        public IEnumerable<Vector2Int> Clears { get;  set; }
    }
}
