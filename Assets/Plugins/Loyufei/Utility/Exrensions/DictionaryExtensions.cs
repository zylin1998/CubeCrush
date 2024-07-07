using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei
{
    public static class DictionaryExtensions
    {
        public static TValue GetorReturn<TKey, TValue>(
            this Dictionary<TKey, TValue> self,
                 TKey         key,
                 Func<TValue> value)
        {
            return self.TryGetValue(key, out var result) ? result : value.Invoke();
        }

        public static TOutput GetorReturn<TKey, TValue, TOutput>(
            this Dictionary<TKey, TValue> self,
                 TKey                     key,
                 Converter<TValue, TOutput> success,
                 Func<TOutput>              failed)
        {
            return self.TryGetValue(key, out var result) ? success(result) : failed.Invoke();
        }

        public static TValue GetorAdd<TKey, TValue>(
            this Dictionary<TKey, TValue> self,
                 TKey                     key, 
                 Func<TValue> add)
        {
            var exist = self.TryGetValue(key, out var value);

            if (!exist) 
            {
                value = add();

                self.Add(key, value);
            }

            return value;
        }
    }
}
