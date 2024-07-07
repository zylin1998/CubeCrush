using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    public interface ISlot
    {
        public object Content { get; }

        public void SetSlot(object content);
        public void UpdateSlot();
        public void ClearSlot();
    }

    public interface ISlot<TContent> : ISlot 
    {
        public new TContent Content { get; }
        object ISlot.Content => Content;

        public void SetSlot(TContent content);
        void ISlot.SetSlot(object content)
        {
            if (content is TContent c) { SetSlot(c); }
        } 
    }
}