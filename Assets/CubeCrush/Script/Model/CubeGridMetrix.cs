using System;
using System.Linq;
using System.Collections.Generic;

namespace CubeCrush
{
    public class CubeGridMetrix
    {
        private class Row 
        {
            public Row(int min, int max, int region, int header) 
            {
                _Numbers = new();

                var number = header + min - 1;
                for(int r = 0; r < region; r++) 
                {
                    for (int v = min; v < max; v++) 
                    {
                        _Numbers.Add(number);

                        number = number == max - 1 ? min : ++number;
                    }
                }
            }

            private List<int> _Numbers;

            public int this[int index] 
                => index >= 0 && index < _Numbers.Count ? _Numbers[index] : 0;
        }

        public CubeGridMetrix(int min, int max, int size) 
        {
            _Max  = max;
            _Min  = min;
            _Size = size;
            _Rows = new Row[_Size];

            var range  = _Max - _Min;
            var region = _Size / range;

            for (var r = 0; r < region; r++) 
            {
                for(var i = 0; i < range; i++) 
                {
                    _Rows[i + r * range] = new Row(_Min, _Max, region, i + 1);
                }
            }
        }

        private int   _Min;
        private int   _Max;
        private int   _Size;
        private Row[] _Rows;

        public List<int> OffsetX { get; } = new();
        public List<int> OffsetY { get; } = new();

        public int this[int row, int column] 
            => row >= 0 && row < _Rows.Length ? _Rows[row][column] : 0; 

        public bool IsClamp(int row, int column) 
        {
            return row >= 0 && row < OffsetX.Count && column >= 0 && column < OffsetY.Count;
        }

        public int Get(int row, int column) 
        {
            var r = row;
            var c = column;
            
            if (!IsClamp(r, c)) { return 0; }
            
            return this[OffsetX[r], OffsetY[c]];
        }

        public void Reset()
        {
            OffsetX.Clear();
            OffsetY.Clear();

            OffsetX.AddRange(Declarations.EvenlyDistributed(_Min - 1, _Max - 1, _Size / (_Max - _Min)));
            OffsetY.AddRange(Declarations.EvenlyDistributed(_Min - 1, _Max - 1, _Size / (_Max - _Min)));
        }
    }
}
