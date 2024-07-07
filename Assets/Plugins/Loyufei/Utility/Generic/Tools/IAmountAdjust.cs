using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei
{
    public interface IAmountAdjust
    {
        public float Amount { get; }

        public bool Increase(float amount);
        public bool Decrease(float amount);
    }
}
