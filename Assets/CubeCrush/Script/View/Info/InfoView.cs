using System.Collections;
using System.Collections.Generic;
using Loyufei;
using Loyufei.ViewManagement;

namespace CubeCrush
{
    public class InfoView : MonoViewBase, IUpdateGroup
    {
        public IEnumerable<IUpdateContext> Contexts => GetComponentsInChildren<IUpdateContext>();
    }
}
