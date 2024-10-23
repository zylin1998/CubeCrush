using System.Collections;
using System.Collections.Generic;
using Loyufei;
using Loyufei.ViewManagement;

namespace CubeCrush
{
    public class InfoView : MenuBase, IUpdateGroup
    {
        public IEnumerable<IUpdateContext> Contexts => GetComponentsInChildren<IUpdateContext>();
    }
}
