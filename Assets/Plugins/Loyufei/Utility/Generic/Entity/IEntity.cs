using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei 
{
    public interface IEntity : IIdentity
    {
        public object Data { get; }
    }

    public interface IEntity<TData> : IEntity
    {
        public new TData Data { get; }

        object IEntity.Data => Data;
    }
}