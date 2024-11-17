using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using NUnit.Framework;
using Loyufei;

namespace CubeCrush.Tests
{
    [TestFixture]
    public class CubeCrushModelTest : ZenjectUnitTestFixture
    {
        private int _Width  = Declarations.Width;
        private int _Height = Declarations.Height;

        [SetUp]
        public void Binding() 
        {
            Container
                .Bind<CubeGrid>()
                .AsSingle();
            Container
                .Bind<CubeGridQuery>()
                .AsSingle();
            Container
                .Bind<CubeCrushModel>()
                .AsSingle();
        } 

        [Test]
        public void Construct() 
        {
            var grid = Container.Resolve<CubeGrid>();

            grid.ClearAll();

            Assert.AreEqual(_Width * _Height, grid.Capacity);

            var (x, y) = (0, 0);
            for (var i = 0; i < _Width * _Height; i++)
            {
                Assert.AreEqual(true, grid.IsEmpty(x, y));
                
                (x, y) = x == _Width - 1 ? (0, ++y) : (++x, y);
            }

            Declarations.Seeds = new Queue<int>(CreateSeeds(1000));

            var map = grid.InjectEmptyAll().ToArray();

            Assert.AreEqual(Declarations.Width, map.Count());
            Assert.AreEqual(true, map.Length == Declarations.Width * Declarations.Height);

            (x, y) = (0, 0);
            var (r, c)   = (0, 0);
            var (c1, c2) = (0, 0);
            var (l1, l2) = (false, false);
            for (var i = 0; i < _Width * _Height; i++)
            {
                var row    = grid.Get(x, y);
                var column = grid.Get(y, x);

                c1 = r == row    ? ++c1 : 1;
                c2 = c == column ? ++c2 : 1;

                (r, c) = (row, column);

                if (c1 >= 3) { l1 = true; }
                if (c2 >= 3) { l2 = true; }

                (x, y) = x == _Width - 1 ? (0, ++y) : (++x, y);
            }
            
            Assert.AreEqual((false, false), (l1, l2));
        }

        [Test]
        public void PositionSwap() 
        {
            var grid = Container.Resolve<CubeGrid>();

            Declarations.Seeds = new Queue<int>(CreateSeeds(1000));

            grid.ClearAll();
            grid.InjectEmptyAll().ToArray();

            var (p1, p2) = ((3, 4), (3, 5));

            var get1 = grid.Get(p1.Item1, p1.Item2);
            var get2 = grid.Get(p2.Item1, p2.Item2);

            grid.Swap(new(p1.Item1, p1.Item2), new(p2.Item1, p2.Item2));

            var get3 = grid.Get(p1.Item1, p1.Item2);
            var get4 = grid.Get(p2.Item1, p2.Item2);
            
            Assert.AreEqual(get1, get4);
            Assert.AreEqual(get2, get3);
        }

        [Test]
        public void ClearAndSwap() 
        {
            var grid = Container.Resolve<CubeGrid>();

            Declarations.Seeds = new Queue<int>(CreateSeeds(1000));

            grid.ClearAll();
            grid.InjectEmptyAll().ToArray();

            var positions = new Vector2Int[]
            {
                new(3, 4), new(4, 4), new(5, 4), new(6, 4), new(7, 4), new(4, 5), new(4, 6),
            };

            var sortedposi = new Vector2Int[]
            {
                new(3, 0), new(4, 0), new(5, 0), new(6, 0), new(7, 0), new(4, 1), new(4, 2),
            };

            positions.ForEach(posi => grid.Clear(posi.x, posi.y));

            grid.SwapEmptyAll();

            Assert.AreEqual(true, sortedposi.All(posi => grid.IsEmpty(posi.x, posi.y)));

            var filledUp = grid.InjectEmptyAll().ToArray();
            
            Assert.AreEqual(7   , filledUp.Length);
            Assert.AreEqual(true, sortedposi.All(posi => filledUp.Any(drop => drop.offset == posi)));
        }

        [Test]
        public void ModelConstruct() 
        {
            var model = Container.Resolve<CubeCrushModel>();
            var query = model.Query;

            Declarations.Seeds = new Queue<int>(CreateSeeds(1000));

            model.Start();
            
            Assert.AreEqual(Declarations.Width, query.InsertCubes.GroupBy(k => k.offset.x).Count());
            
            var swap = model.Swap(new(13, 1), new(13, 2));
            
            Assert.AreEqual(true, swap);

            Declarations.Seeds = new Queue<int>(CreateSeeds(1000));

            model.FillEmpty();

            var inserts = query.InsertCubes.ToArray();

            Assert.AreEqual(3, query.Clears.Count());
            Assert.AreEqual(3, inserts.Length);
            
            var check = model.CheckFilled();

            Assert.AreEqual(true, check);

            Declarations.Seeds = new Queue<int>(CreateSeeds(1000));
            
            model.FillEmpty();

            inserts = query.InsertCubes.ToArray();

            Assert.AreEqual(6, query.Clears.Count());
            Assert.AreEqual(6, inserts.Length);
            
            var gameOver = model.GameOver();

            Assert.AreEqual(false, gameOver);
        }

        private int[] CreateSeeds(int length) 
        {
            var seed = 1;
            
            return new int[1000].Select(n => seed++).ToArray();
        }

        private void Showgrid(CubeGrid grid)
        {
            string str = string.Empty;

            var (x, y) = (0, 0);

            for (int i = 0; i < _Width * _Height; i++)
            {
                str += grid.Get(x, y) + " ";

                if (x == _Width - 1) { str += "\n"; }

                (x, y) = x == _Width - 1 ? (0, ++y) : (++x, y);
            }

            Debug.Log(str);
        }
    }
}