using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Loyufei;
using Loyufei.ViewManagement;

namespace CubeCrush
{
    public class InfoView : MenuBase, IUpdateGroup, IUpdateContext
    {
        [SerializeField]
        private TextMeshProUGUI _Score;

        public object Id => Declarations.Score;

        public IEnumerable<IUpdateContext> Contexts => new IUpdateContext[] { this };

        public void SetContext(object value) 
        {
            _Score.SetText(value.ToString());
        }
    }
}
