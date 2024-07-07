using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Loyufei
{
    public abstract class BindableAsset : ScriptableObject, IBindableAsset
    {
        public abstract void BindToContainer(DiContainer container, object group = null);
    }
}