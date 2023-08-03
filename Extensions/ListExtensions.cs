using System;
using System.Collections.Generic;
using System.Linq;
using Venn.Comparers;

namespace Venn.Extensions
{
    public static class ListExtensions
    {
        #region methods Replace
        public static IList<T> Replace<T>(this IList<T> replaceableCollection, IList<T> newCollection, VennComparer<T> comparer)
        {
            if (newCollection == null)
                throw new ArgumentNullException(nameof(newCollection));

            newCollection.ToList().ForEach(newItem =>
            {
                Replace(replaceableCollection, newItem, comparer);
            });

            return replaceableCollection;
        }

        public static void Replace<T>(this IList<T> collection, T newItem, VennComparer<T> comparer)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (newItem == null)
                throw new ArgumentNullException(nameof(newItem));

            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            collection.ToList().ForEach(oldItem =>
            {
                collection.ReplaceItem(oldItem, newItem, comparer);
            });
        }

        private static bool ReplaceItem<T>(this IList<T> collection, T oldItem, T newItem, VennComparer<T> comparer)
        {
            bool result = false;
            if (comparer.Equals(oldItem, newItem))
            {
                try
                {
                    collection[collection.IndexOf(oldItem)] = newItem;
                    result = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return result;
        }

        [Obsolete]
        public static IList<T> Replace<T>(this IList<T> replaceableCollection, IList<T> newCollection, Func<T, T, bool> predicate)
        {
            if (newCollection == null)
                throw new ArgumentNullException(nameof(newCollection));

            newCollection.ToList().ForEach(newItem =>
            {
                Replace(replaceableCollection, newItem, predicate);
            });

            return replaceableCollection;
        }

        [Obsolete]
        public static void Replace<T>(this IList<T> collection, T newItem, Func<T,T,bool> predicate)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (newItem == null)
                throw new ArgumentNullException(nameof(newItem));

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            collection.ToList().ForEach(oldItem =>
            {
                collection.ReplaceItem(oldItem, newItem, predicate);
            });
        }

        [Obsolete]
        private static bool ReplaceItem<T>(this IList<T> collection, T oldItem, T newItem, Func<T,T,bool> predicate)
        {
            bool result = false;
            if (predicate(oldItem, newItem))
            {
                try
                {
                    collection[collection.IndexOf(oldItem)] = newItem;
                    result = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return result;
        }
        #endregion

        #region methods Add
        public static IList<T> AddRange<T>(this IList<T> replaceableCollection, IList<T> newCollection, VennComparer<T> comparer)
        {
            if (replaceableCollection == null)
                throw new ArgumentNullException(nameof(replaceableCollection));

            if (newCollection == null)
                throw new ArgumentNullException(nameof(newCollection));

            newCollection.ToList().ForEach(newItem =>
            {
                replaceableCollection.Add<T>(newItem, comparer);
            });

            return replaceableCollection;
        }

        public static void Add<T>(this IList<T> collection, T newItem, VennComparer<T> comparer)
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
                collection.AddItem<T>(newItem, oldItem, comparer);
            });
        }

        private static void AddItem<T>(this IList<T> collection, T newItem, T oldItem, VennComparer<T> comparer)
        {
            if (oldItem != null && comparer != null)
            {
                if (comparer.Equals(oldItem, newItem))
                {
                    collection.Add(newItem);
                }
            }
            else
            {
                collection.Add(newItem);
            }
        }

        [Obsolete]
        public static IList<T> AddRange<T>(this IList<T> replaceableCollection, IList<T> newCollection, Func<T, T, bool> predicate = null)
        {
            if (replaceableCollection == null)
                throw new ArgumentNullException(nameof(replaceableCollection));

            if (newCollection == null)
                throw new ArgumentNullException(nameof(newCollection));

            newCollection.ToList().ForEach(newItem =>
            {
                replaceableCollection.Add<T>(newItem, predicate);
            });

            return replaceableCollection;
        }

        [Obsolete]
        public static void Add<T>(this IList<T> collection, T newItem, Func<T, T, bool> predicate = null)
        {
            if (!collection.Any())
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

        [Obsolete]
        private static void AddItem<T>(this IList<T> collection, T newItem, T oldItem, Func<T, T, bool> predicate)
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
        #endregion

        [Obsolete]
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
                if(replaceableCollection.IndexOf(newItem) != -1)
                {
                    replaceableCollection.Add(newItem);
                }
            });

            return replaceableCollection;
        }

        [Obsolete]
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
        public static IList<T> ReplaceOrAdd<T>(this IList<T> replaceableCollection, IList<T> newCollection, VennComparer<T> comparer)
        {
            replaceableCollection.Replace(newCollection, comparer);

            newCollection
                .ToList()
                .ForEach(newItem =>
                {
                    if (replaceableCollection.IndexOf(newItem) != -1)
                    {
                        replaceableCollection.Add(newItem);
                    }
                });

            return replaceableCollection;
        }

        #region Slice
        public static IList<T> Slice<T>(this IList<T> collection, int start = 0, int end = 1, int step = 1, int taken = 1)
        {
            if(collection == null)
                throw new ArgumentNullException(nameof(collection));

            List<T> result = new List<T>();

            for (int i = start; i < end; i += step)
            {
                if (i == start)
                {
                    result.Add(collection[start]);
                }
                else
                {
                    result.AddRange(collection.Skip(step + i).Take(taken));
                }
            }

            return result;
        }

        public static IList<T> Slice<T>(this IList<T> collection, int start = 0, int end = 1)
        {
            return Slice<T>(collection, start, end, 1, 1);
        }

        public static IList<T> Slice<T>(this IList<T> collection, int start = 0)
        {
            return Slice<T>(collection, start, collection.Count);
        }

        #endregion
    }
}
