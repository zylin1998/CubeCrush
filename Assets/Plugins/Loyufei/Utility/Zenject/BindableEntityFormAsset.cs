using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using UnityEngine;

namespace Loyufei
{
    public abstract class BindableEntityFormAsset<TItem, TEntity> : 
        BindableAsset, 
        IEntityForm<TItem, TEntity> where TEntity : IEntity<TItem>
    {
        [SerializeField]
        protected List<TEntity> _Entities;

        public IEntity<TItem> this[object identity]
            => _Entities.FirstOrDefault(e => Equals(e.Identity, identity));

        public IEnumerator<IEntity<TItem>> GetEnumerator()
            => _Entities.OfType<IEntity<TItem>>().GetEnumerator();

        IEnumerator<IEntity> IEnumerable<IEntity>.GetEnumerator()
            => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
