using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei
{
    public interface IReposit : IEntity
    {
        public void Preserve(object data);

        public void Release();

        public void SetIdentify(object    identify);
        public void SetIdentify(IIdentity identify);
    }

    public interface IReposit<T> : IReposit, IEntity<T>
    {
        public void Preserve(T data);

        void IReposit.Preserve(object data)
        {
            if (data is T t) { Preserve(t); }
        }
    }
}
