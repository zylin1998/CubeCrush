using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei
{
    public interface IRepositoryInfo
    {
        public List<(int index, string identify, object data)> Preserves { get; }

        public List<(int index, object data)> this[string identify] { get; }
        public (int index, string identify, object data) this[int index] { get; }

        public int Capacity { get; }
        public int Empty    { get; }
    }
}
