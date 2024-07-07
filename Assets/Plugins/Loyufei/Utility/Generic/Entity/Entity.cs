using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

namespace Loyufei 
{
    public class Entity<TData> : IEntity<TData> 
    {
        public Entity(object identity, TData data)
        {
            Identity = identity;
            Data     = data;
        }

        public object Identity { get; }
        public TData  Data     { get; }
    }

    [Serializable]
    public class Entity<TIdentity, TData> : IEntity<TData>
    {
        public Entity(TIdentity identity, TData data) 
        {
            _Identity = identity;
            _Data     = data;
        }

        [SerializeField]
        protected TIdentity _Identity;
        [SerializeField]
        protected TData     _Data;

        public object    Identity => _Identity;
        public TData     Data     =>_Data;
    }

    public class EntityAsset<TIdentity, TData> : ScriptableObject, IEntity<TData> 
    {
        [SerializeField]
        protected TIdentity _Identity;
        [SerializeField]
        protected TData _Data;

        public TIdentity Identity => _Identity;
        public TData     Data     => _Data;

        object IIdentity.Identity => _Identity;
    }
}
