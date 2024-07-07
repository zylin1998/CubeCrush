using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Loyufei.ViewManagement
{
    public class ButtonListener : MonoListenerAdapter<Button>
    {
        public override void AddListener(Action<object> callBack)
        {
            Listener.onClick.AddListener(() => callBack.Invoke(Id));
        }
    }
}