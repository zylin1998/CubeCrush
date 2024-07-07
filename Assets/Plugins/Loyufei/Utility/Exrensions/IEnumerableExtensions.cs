using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    public static class IEnumerableExtensions
    {
        #region Type Mapping

        public static Dictionary<Type, List<TItem>> TypeMapping<TItem>(
            this IEnumerable<TItem> self,
                 Func<TItem, Type>  typeSelector)
            => self.TypeMapping<TItem, TItem, Dictionary<Type, List<TItem>>>(typeSelector, value => value);

        public static Dictionary<Type, List<TValue>> TypeMapping<TItem, TValue>(
            this IEnumerable<TItem>  self,
                 Func<TItem, Type>   typeSelector,
                 Func<TItem, TValue> valueSelector)
            => self.TypeMapping<TItem, TValue, Dictionary<Type, List<TValue>>>(typeSelector, valueSelector);

        public static TDictionary TypeMapping<TItem, TDictionary>(
            this IEnumerable<TItem> self,
                 Func<TItem, Type>  typeSelector) where TDictionary : Dictionary<Type, List<TItem>>
            => self.TypeMapping<TItem, TItem, TDictionary>(typeSelector, value => value);

        public static TDictionary TypeMapping<TItem, TValue, TDictionary>(
            this IEnumerable<TItem> self,
                 Func<TItem, Type> typeSelector,
                 Func<TItem, TValue> valueSelector) where TDictionary : Dictionary<Type, List<TValue>>
            => self.Mapping<TItem, Type, TValue, TDictionary>(typeSelector, valueSelector);

        #endregion

        #region Mapping

        public static Dictionary<TKey, List<TItem>> Mapping<TItem, TKey>(
            this IEnumerable<TItem> self,
                 Func<TItem, TKey>  keySelector)
            => self.Mapping<TItem, TKey, TItem, Dictionary<TKey, List<TItem>>>(keySelector, value => value);

        public static Dictionary<TKey, List<TValue>> Mapping<TItem, TKey, TValue>(
            this IEnumerable<TItem>  self,
                 Func<TItem, TKey>   keySelector,
                 Func<TItem, TValue> valueSelector)
            => self.Mapping<TItem, TKey, TValue, Dictionary<TKey, List<TValue>>>(keySelector, valueSelector);

        public static TDictionary Mapping<TItem, TKey, TDictionary>(
            this IEnumerable<TItem> self,
                 Func<TItem, TKey>  keySelector) where TDictionary : Dictionary<TKey, List<TItem>>
            => self.Mapping<TItem, TKey, TItem, TDictionary>(keySelector, value => value);

        public static TDictionary Mapping<TItem, TKey, TValue, TDictionary>(
            this IEnumerable<TItem>  self,
                 Func<TItem, TKey>   keySelector,
                 Func<TItem, TValue> valueSelector) where TDictionary : Dictionary<TKey, List<TValue>>
        {
            var mapping = Activator.CreateInstance<TDictionary>();

            Debug.Assert(self          != null, "Self must not be Null.");
            Debug.Assert(keySelector   != null, "Type selector must not be Null.");
            Debug.Assert(valueSelector != null, "Value selector must not be Null.");

            foreach (var item in self)
            {
                var key   = keySelector.Invoke(item);
                var value = valueSelector.Invoke(item);

                if (mapping.TryGetValue(key, out var list))
                {
                    list.Add(value);
                }

                else { mapping.Add(key, new List<TValue>() { value }); }
            }

            return mapping;
        }

        #endregion

        public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            foreach (var item in self) { action?.Invoke(item); }
        }

        #region First or Default

        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> self, TSource isDefault) 
        {
            return self.FirstOrDefault() ?? isDefault;
        }

        public static TSource FirstOrDefault<TSource>(
            this IEnumerable<TSource> self, 
                 Func<TSource, bool>  predicate,
                 TSource              isDefault)
        {  
            return self.FirstOrDefault(predicate) ?? isDefault;
        }

        #endregion
    }
}
