using System;
using System.Collections.Generic;
using System.Linq;

namespace Venn.Extensions
{
    public static class ListExtensions
    {
        public static IList<T> Replace<T>(this IList<T> replaceableCollection, IList<T> newCollection, Func<T, T, bool> predicate)
        {
            newCollection.ToList().ForEach(newItem =>
            {
                Replace(replaceableCollection, newItem, predicate);
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

        private static bool ReplaceItem<T>(this IList<T> collection, T oldItem, T newItem, Func<T,T,bool> predicate)
        {
            bool result = false;
            if (predicate(oldItem, newItem))
            {
                collection[collection.IndexOf(oldItem)] = newItem;
                result = true;
            }

            return result;
        }

        public static IList<T> AddRange<T>(this IList<T> replaceableCollection, IList<T> newCollection, Func<T, T, bool> predicate = null)
        {
            newCollection.ToList().ForEach(newItem =>
            {
                replaceableCollection.Add<T>(newItem, predicate);
            });

            return replaceableCollection;
        }

        public static void Add<T>(this IList<T> collection, T newItem, Func<T, T, bool> predicate = null)
        {
            if(!collection.Any())
            {
                collection.Add(newItem);
                return;
            }

            collection
                .ToList()
                .ForEach(oldItem =>
            {
                collection.AddItem<T>(newItem, oldItem, predicate);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="oldItem"></param>
        /// <param name="newItem"></param>
        /// <param name="predicate"></param>
        private static void AddItem<T>(this IList<T> collection, T newItem, T oldItem, Func<T,T,bool> predicate)
        {
            if (oldItem != null && predicate != null)
            {
                if (predicate(oldItem, newItem))
                {
                    collection.Add(newItem);
                }
            }
            else
            {
                collection.Add(newItem);
            }
        }

        /// <summary>
        /// The replaceableCollection items are replaced by the newCollection items if they meet the replaceblePredict condition,
        /// otherwise the newCollection items are inserted into the repleableCollection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="replaceableCollection">Collection where the newCollection items will be updated or inserted</param>
        /// <param name="newCollection">Collection containing the new items</param>
        /// <param name="replacePredicate">Condition that the items to be replaced in the replaceable collection must meet,
        /// otherwise they will be inserted into the collection</param>
        /// <returns></returns>
        public static IList<T> ReplaceOrAdd<T>(this IList<T> replaceableCollection, IList<T> newCollection, Func<T, T, bool> replacePredicate)
        {
            replaceableCollection.Replace(newCollection, replacePredicate);

            newCollection
                .ToList()
                .ForEach(newItem =>
            {
                if (!replaceableCollection.Contains(newItem))
                {
                    replaceableCollection.Add(newItem);
                }
            });

            return replaceableCollection;
        }
    }

}
