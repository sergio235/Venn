using System;
using System.Collections.Generic;
using System.Linq;

namespace Venn.Extensions
{
    public static class ListExtensions
    {
        public static IList<T> Replace<T>(this IList<T> replaceableCollection, IList<T> newCollection, Func<T,T,bool> predicate)
        {
            newCollection.ToList().ForEach(itemB =>
            {
                Replace(replaceableCollection, itemB, predicate);
            });

            return replaceableCollection;
        }

        public static void Replace<T>(this IList<T> collection, T newItem, Func<T,T,bool> predicate)
        {
            collection.ToList().ForEach(oldItem =>
            {
                collection.ReplaceItem(oldItem, newItem, predicate);
            });
        }

        public static void ReplaceItem<T>(this IList<T> collection, T oldItem, T newItem, Func<T,T,bool> predicate)
        {
            if (predicate(oldItem, newItem))
            {
                collection[collection.IndexOf(oldItem)] = newItem;
            }
        }
    }

}
