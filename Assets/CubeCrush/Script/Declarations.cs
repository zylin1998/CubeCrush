using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeCrush
{
    public static class Declarations
    {
        public const string CubeCrush = "CubeCrush";
        public const string Score     = "Score";
        public const string Turn      = "Turn";

        //All fields, properties, methods below can be declared as game setting class.

        public const int    Width     = 10;
        public const int    Height    = 10;
        public const int    MaxTurn   = 40;

        public static Color[] TypeColor { get; } = new Color[5]
        {
            Color.clear, Color.red, Color.blue, Color.cyan, Color.yellow,
        };

        public static int[] ScoreRate = new int[7]
        {
            3, 5, 9, 15, 23, 33, 45
        };

        public static int GetScore(int count)
        {
            return count >= 3 ? 3 * ScoreRate[count >= ScoreRate.Length ? ScoreRate.Length - 1 : count] : 0;
        }

        public static Queue<int> Seeds { get; set; } = new Queue<int>();

        public static List<int> GetRandoms(int min, int max, int length) 
        {
            var list = new List<int>();

            var repeat = 0;
            for (var i = 0; i < length;)
            {
                if (repeat >= Mathf.Pow(length, 3)) { break; }

                if (Seeds.Any()) { Random.InitState(Seeds.Dequeue()); }

                var random = Random.Range(min, max);

                if (list.Contains(random)) 
                {
                    repeat++;

                    continue; 
                }

                i++;

                list.Add(random);
            }

            return list;
        }

        public static List<int> EvenlyDistributed(int min, int max, int region)
        {
            var list  = new List<int>();
            var range = max - min;

            for (int i = 0; i < region; i++) 
            {
                list.AddRange(GetRandoms(min, max, range).ConvertAll(num => num + i * range));
            }

            return list;
        }
    }
}
