using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei;

namespace CubeCrush
{
    public class CubeCrushModel
    {
        public CubeCrushModel(CubeGrid grid, Report report, CubeGridQuery query)
        {
            Grid   = grid;
            Query  = query;
            Report = report;
        }

        public CubeGrid      Grid   { get; }
        public CubeGridQuery Query  { get; }
        public Report        Report { get; }

        public void Start()
        {
            Grid.ClearAll();

            Query.InsertCubes = Grid.InjectEmptyAll().ToArray();
        }

        public bool Swap(Vector2Int offset1, Vector2Int offset2)
        {
            Grid.Swap(offset1, offset2);

            var check1 = CheckLine(offset1);
            var check2 = CheckLine(offset2);
            var swap   = check1 || check2;

            if (!swap) { Grid.Swap(offset1, offset2); }

            return swap;
        }

        public void FillEmpty() 
        {
            Query.Clears = GetEmpties().ToArray();
            
            Grid.SwapEmptyAll();

            Query.InsertCubes = Grid.InjectEmptyAll().ToArray();
        }

        public bool GameOver() 
        {
            var list = new List<Vector2Int>();

            var (x, y) = (0, 0);
            for (int i = 0; i < (Declarations.Width) * (Declarations.Height); i++)
            {
                list.Add(new(x, y));

                (x, y) = x == Declarations.Width - 1 ? (0, ++y) : (++x, y);
            }

            return list.Select(Preview).ToArray().All(p => !p);
        }

        private bool CheckLine(Vector2Int offset) 
        {
            var horizontal = GetLine(offset, new(1, 0));
            var vertical   = GetLine(offset, new(0, 1));

            var horiClear  = ClearLine(horizontal);
            var vertClear  = ClearLine(vertical);

            ReportScore(new Vector2Int[][] { horizontal, vertical });
            
            return horiClear || vertClear;
        }

        public bool CheckFilled () 
        {
            var list = new List<(Vector2Int offset, int type)>();

            var (x, y) = (0, 0);
            for (int i = 0; i < (Declarations.Width) * (Declarations.Height); i++)
            {
                var offset = new Vector2Int(x, y);
                var type   = Grid.Get(x, y);
                var horizontal = GetLine(offset, new(1, 0));
                var vertical   = GetLine(offset, new(0, 1));

                if (horizontal.Length >= 3 || vertical.Length >= 3) 
                {
                    list.Add((offset, type)); 
                }

                (x, y) = x == Declarations.Width - 1 ? (0, ++y) : (++x, y);
            }

            for (var i = 1; i < Declarations.TypeColor.Length; i++) 
            {
                Report.ReportScore(Declarations.GetScore(list.Sum(info => info.type == i ? 1 : 0)));
            }

            return ClearLine(list.ConvertAll(c => c.offset));
        }

        private bool Preview(Vector2Int offset)
        {
            var horizontal  = PreviewLine(offset, new(1, 0));
            var vertical    = PreviewLine(offset, new(0, 1));

            var horiPreview = CheckPreview(offset, Vector2Int.up   , horizontal);
            var vertPreview = CheckPreview(offset, Vector2Int.right, vertical);

            return horiPreview || vertPreview;
        }

        private bool ClearLine(IEnumerable<Vector2Int> line) 
        {
            if (line.Count() < 3) { return false; }

            line.ForEach(posi => Grid.Clear(posi.x, posi.y));

            return true;
        }

        private bool CheckPreview(Vector2Int offset, Vector2Int direct, Vector2Int[] line) 
        {
            if (line.Length < 2) { return false; }

            var value = Grid.Get(line[0].x, line[0].y);

            var offset1 = offset + direct;
            var offset2 = offset - direct;

            return Grid.Get(offset1.x, offset1.y) == value || Grid.Get(offset2.x, offset2.y) == value;
        }

        private Vector2Int[] PreviewLine(Vector2Int offset, Vector2Int direct) 
        {
            var line = GetPositions(offset, direct, true).ToArray();

            return Continuous(line, 2);
        }

        private Vector2Int[] GetLine(Vector2Int offset, Vector2Int direct) 
        {
            var line = GetPositions(offset, direct, false).ToArray();

            return Continuous(line, 3);
        }

        private Vector2Int[] Continuous(Vector2Int[] positions, int continuous) 
        {
            var list = new List<Vector2Int>();
            var type = 0;

            foreach (var posi in positions)
            {
                var grid = Grid.Get(posi.x, posi.y);
                
                if (grid <= 0)    { continue; }

                if (type != grid) 
                {
                    if (list.Count >= continuous) { break; }
                
                    list.Clear(); 
                }

                list.Add(posi);

                type = grid;
            }
            
            return list.ToArray();
        }

        private IEnumerable<Vector2Int> GetPositions(Vector2Int offset, Vector2Int direct, bool ignoreOffset) 
        {
            for (var i = -2; i <= 2; i++) 
            {
                if (ignoreOffset && i == 0) { continue; }

                yield return offset + direct * i;
            }
        }

        private IEnumerable<Vector2Int> GetEmpties() 
        {
            var (x, y) = (0, 0);
            for (int i = 0; i < Declarations.Width * Declarations.Height; i++) 
            {
                if (Grid.IsEmpty(x, y)) { yield return new(x, y); }

                (x, y) = x == Declarations.Width - 1 ? (0, ++y) : (++x, y);
            }
        }

        private void ReportScore(IEnumerable<Vector2Int[]> lines) 
        {
            var score = lines.Sum(line => Declarations.GetScore(line.Length));

            if (score <= 0) { return; }

            Report.ReportScore(score);
        }
    }
}
