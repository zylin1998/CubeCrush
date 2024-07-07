using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei 
{
    public interface IVisual
    {
        public bool Enable { get; set; }

        public void Refresh();

        public static IVisual Default => new DefaultVisual();

        public struct DefaultVisual : IVisual
        {
            public bool Enable { get; set; }

            public void Refresh() { }
        }
    }
}
