using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZippingEasy.Common.Utils
{
    public static class IListExtensions
    {
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            foreach (T item in items.EmptyIfNull())
            {
                list.Add(item);
            }
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
                return new List<T>();

            return collection;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> self, Func<TSource, TKey> selector)
            => self.GroupBy(selector).Select(f => f.First());
    }
}
