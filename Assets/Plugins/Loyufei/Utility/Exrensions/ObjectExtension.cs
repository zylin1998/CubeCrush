using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei
{
    public static class ObjectExtension
    {
        public static bool IsDefault<T>(this T obj) 
        {
            return object.Equals(obj, default(T));
        }

        public static T IsDefaultandReturn<T>(this T obj, T isDefault)
        {
            return object.Equals(obj, default(T)) ? isDefault : obj;
        }

        public static bool IsEqual(this object object1, object object2) 
        {
            if (object1.IsDefault() && object2.IsDefault()) { return true; }

            if (object1.IsDefault() || object2.IsDefault()) { return false; }

            return object.Equals(object1, object2);
        }

        public static TOutput To<TOutput>(this object obj) 
        {
            return obj is TOutput output ? output : default(TOutput);
        }
        
        public static bool TryTo<TInput, TOutput>(this TInput input, out TOutput output)
        {
            output = input.To<TOutput>();

            return !output.IsDefault();
        }

        public static TOutput Convert<TInput, TOutput>(this TInput obj, Converter<TInput, TOutput> converter)
        {
            return converter.IsDefault() ? default(TOutput) : converter.Invoke(obj);
        }
    }
}
