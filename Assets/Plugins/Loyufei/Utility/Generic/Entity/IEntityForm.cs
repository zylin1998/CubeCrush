using System.Collections;
using System.Collections.Generic;

namespace Loyufei 
{
    public interface IEntityForm : IEnumerable<IEntity>
    {
        public IEntity this[object identity] { get; }
    }

    public interface IEntityForm<TItem, TEntity> : IEntityForm, IEnumerable<IEntity<TItem>> where TEntity : IEntity<TItem>
    {
        public new IEntity<TItem> this[object identity] { get; }

        IEntity IEntityForm.this[object identity]
            => this[identity];
    }
}