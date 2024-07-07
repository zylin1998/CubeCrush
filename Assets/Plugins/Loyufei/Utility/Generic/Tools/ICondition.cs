using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Loyufei
{
    public interface ICondition
    {
        public Func<bool> Condition { get; }
    }
}
