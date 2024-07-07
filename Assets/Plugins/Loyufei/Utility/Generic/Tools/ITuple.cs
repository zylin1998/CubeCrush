using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei
{
    #region Tuple2

    public interface ITuple2
    {
        public object Item1 { get; }
        public object Item2 { get; }
    }

    public interface ITuple<TItem1, TItem2> : ITuple2 
    {
        public new TItem1 Item1 { get; }
        public new TItem2 Item2 { get; }

        object ITuple2.Item1 => Item1;
        object ITuple2.Item2 => Item1;
    }

    #endregion

    #region Tuple3

    public interface ITuple3
    {
        public object Item1 { get; }
        public object Item2 { get; }
        public object Item3 { get; }
    }

    public interface ITuple<TItem1, TItem2, TItem3> : ITuple3
    {
        public new TItem1 Item1 { get; }
        public new TItem2 Item2 { get; }
        public new TItem3 Item3 { get; }

        object ITuple3.Item1 => Item1;
        object ITuple3.Item2 => Item1;
        object ITuple3.Item3 => Item1;
    }

    #endregion

    #region Tuple4

    public interface ITuple4
    {
        public object Item1 { get; }
        public object Item2 { get; }
        public object Item3 { get; }
        public object Item4 { get; }
    }

    public interface ITuple<TItem1, TItem2, TItem3, TItem4> : ITuple4
    {
        public new TItem1 Item1 { get; }
        public new TItem2 Item2 { get; }
        public new TItem3 Item3 { get; }
        public new TItem4 Item4 { get; }

        object ITuple4.Item1 => Item1;
        object ITuple4.Item2 => Item1;
        object ITuple4.Item3 => Item1;
        object ITuple4.Item4 => Item1;
    }

    #endregion

    #region Tuple5

    public interface ITuple5
    {
        public object Item1 { get; }
        public object Item2 { get; }
        public object Item3 { get; }
        public object Item4 { get; }
        public object Item5 { get; }
    }

    public interface ITuple<TItem1, TItem2, TItem3, TItem4, TItem5> : ITuple5
    {
        public new TItem1 Item1 { get; }
        public new TItem2 Item2 { get; }
        public new TItem3 Item3 { get; }
        public new TItem4 Item4 { get; }
        public new TItem4 Item5 { get; }

        object ITuple5.Item1 => Item1;
        object ITuple5.Item2 => Item1;
        object ITuple5.Item3 => Item1;
        object ITuple5.Item4 => Item1;
        object ITuple5.Item5 => Item1;
    }

    #endregion
}
