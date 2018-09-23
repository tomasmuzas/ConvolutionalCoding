using System;
using System.Collections.Generic;

namespace ConvolutionalCodes.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> list, IEnumerable<T> toPrepend)
        {
            foreach (T t in toPrepend)
            {
                yield return t;
            }
            foreach (T t in list)
            {
                yield return t;
            }
        }
    }
}
