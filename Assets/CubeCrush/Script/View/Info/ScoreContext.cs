using Loyufei;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CubeCrush
{
    public class ScoreContext : MonoBehaviour, IUpdateContext
    {
        [SerializeField]
        private TextMeshProUGUI _Score;

        public object Id => Declarations.Score;

        public void SetContext(object value)
        {
            _Score.SetText(value.ToString());
        }
    }
}