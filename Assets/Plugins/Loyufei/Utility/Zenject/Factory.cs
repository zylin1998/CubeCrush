using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Loyufei
{
    public class Factories<TIdentifier, TItem> : IFactory<TIdentifier, TItem>
    {
        public Factories(IEnumerable<Factory<TIdentifier, TItem>> factories) 
        {
            Dic = factories.ToDictionary(key => key.Identifier.To<TIdentifier>());
        }

        public Dictionary<TIdentifier, Factory<TIdentifier, TItem>> Dic { get; }

        public TItem Create(TIdentifier type) 
        {
            return Dic.TryGetValue(type, out var factory) ? factory.Create() : default;
        }
    }

    public class Factory<T> : PlaceholderFactory<T>, IIdentifiedFactory<T>
    {
        public Factory(object identifier) 
        {
            Identifier = identifier;
        }

        public object Identifier { get; }
    }

    public class Factory<TIdentifier, TItem> : PlaceholderFactory<TItem>, IIdentifiedFactory<TIdentifier, TItem>
    {
        public Factory(TIdentifier identifier)
        {
            Identifier = identifier;
        }

        public TIdentifier Identifier { get; }
    }

    public interface IIdentifiedFactory<T> : IFactory<T> 
    {
        public object Identifier { get; }
    }

    public interface IIdentifiedFactory<TIdentifier, TItem> : IIdentifiedFactory<TItem>
    {
        public new TIdentifier Identifier { get; }

        object IIdentifiedFactory<TItem>.Identifier => Identifier;
    }
}