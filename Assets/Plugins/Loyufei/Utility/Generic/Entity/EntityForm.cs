using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    public class EntityForm<TItem, TEntity> : IEntityForm<TItem, TEntity> where TEntity : IEntity<TItem>
    {
        public EntityForm(IEnumerable<TEntity> entities) 
        {
            Dictionary = entities.ToDictionary(e => e.Identity);
        }

        public Dictionary<object, TEntity> Dictionary { get; }

        public IEntity<TItem> this[object identity] 
            => Dictionary.TryGetValue(identity, out var entity) ? entity : default;

        public IEnumerator<IEntity<TItem>> GetEnumerator() 
            => Dictionary.Values.OfType<IEntity<TItem>>().GetEnumerator();

        IEnumerator<IEntity> IEnumerable<IEntity>.GetEnumerator() 
            => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() 
            => GetEnumerator();
    }

    public class EntityForm<TItem> : EntityForm<TItem, Entity<TItem>> 
    {
        public EntityForm(IEnumerable<Entity<TItem>> entities) : base(entities)
        {
            
        }
    }

    public class EntityFormAsset<TItem, TEntity> : 
        ScriptableObject, 
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
