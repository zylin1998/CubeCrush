using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei;

namespace CubeCrush
{
    public class CubeGrid : RepositoryBase<int, int>
    {
        public CubeGrid() : base(Declarations.Width * Declarations.Width) 
        {
            Metrix = new CubeGridMetrix(1, 5, 20);

            var (x, y) = (0, 0);
            _Reposits.ForEach(r =>
            {
                r.SetIdentify(x + y * Declarations.Width);

                (x, y) = x == Declarations.Width - 1 ? (0, ++y) : (++x, y);
            });
        }

        public IReposit<int> this[int x, int y] 
            => SearchAt(x + y * Declarations.Width);

        public CubeGridMetrix Metrix { get; }

        public bool IsClamp(int x, int y) 
        {
            return x >= 0 && x < Declarations.Width && y >= 0 && y < Declarations.Height;
        }

        public bool IsEmpty(int x, int y) 
        {
            if (!IsClamp(x, y)) { return false; }

            return this[x, y].Data <= 0;
        }

        public int Get(int x, int y) 
        {
            if (!IsClamp(x, y)) { return -1; }

            return this[x, y].Data;
        }

        public bool Clear(int x, int y) 
        {
            if (!IsClamp(x, y)) { return false; }
            
            this[x, y].Preserve(0);

            return true;
        }

        public void ClearAll() 
        {
            _Reposits.ForEach(r => r.Preserve(0));
        }

        public void Swap(Vector2Int offset1, Vector2Int offset2) 
        {
            if (!IsClamp(offset1.x, offset1.y) || !IsClamp(offset2.x, offset2.y)) { return; }

            var reposit1 = this[offset1.x, offset1.y];
            var reposit2 = this[offset2.x, offset2.y];

            var temp = reposit1.Data;

            reposit1.Preserve(reposit2.Data);
            reposit2.Preserve(temp);
        }

        public void SwapEmpty(int x, int y) 
        {
            if (!IsEmpty(x, y)) { return; }

            for(var c = y - 1; c >= 0; c--) 
            {
                Swap(new(x, c + 1), new(x, c));
            }
        }

        public void SwapEmptyAll()
        {
            for (var y = 1; y < Declarations.Height; y++) 
            {
                for(var x = 0; x < Declarations.Width; x++) 
                {
                    if (!IsEmpty(x, y)) { continue; }

                    SwapEmpty(x, y);
                }
            }
        }

        public (Vector2Int offset, int value) InjectEmpty(int x, int y) 
        {
            if (!IsEmpty(x, y)) { return default; }

            var reposit = this[x, y];
            var value   = Metrix.Get(x, y);

            reposit.Preserve(value);

            return (new(x, y), reposit.Data);
        }

        public IEnumerable<(Vector2Int offset, int value)> InjectEmptyAll() 
        {
            Metrix.Reset();

            var (x, y) = (0, 0);
            for(var i = 0; i < Declarations.Width * Declarations.Height; i++) 
            {
                if (IsEmpty(x, y)) 
                {
                    yield return InjectEmpty(x, y);
                }

                (x, y) = x == Declarations.Width - 1 ? (0, ++y) : (++x, y);
            }
        }
    }
}