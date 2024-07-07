using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyufei
{
    public interface ISearch
    {
        public object Search(object identify);
    }

    public interface ISearch<TInput, TOutput> : ISearch
    {
        public TOutput Search(TInput identify);

        object ISearch.Search(object identify)
            => identify is TInput input ? Search(input) : default;
    }
}
