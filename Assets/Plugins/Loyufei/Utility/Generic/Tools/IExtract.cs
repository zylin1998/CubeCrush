using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyufei
{
    public interface IExtract
    {
        public object Extract();
    }

    public interface IExtract<TParam1>
    {
        public object Extract(TParam1 param1);
    }
}
