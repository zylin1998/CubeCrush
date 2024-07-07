using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Loyufei
{
    public interface IBindableAsset
    {
        public void BindToContainer(DiContainer container, object group = null);
    }
}
