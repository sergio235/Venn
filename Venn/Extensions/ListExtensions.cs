using System;
using System.Collections.Generic;
using System.Linq;
using Venn.Comparers;

namespace Venn.Extensions
{
    /// <summary>
    /// Extension methods for working with IList collections.
    /// </summary>
    public static class ListExtensions
    {
        #region Replace Methods

        /// <summary>
        /// Replaces items in the target collection with corresponding items from the new collection based on the provided comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collections.</typeparam>
        /// <param name="replaceableCollection">The collection to be modified.</param>
        /// <param name="newCollection">The collection containing new items.</param>
        /// <param name="comparer">The comparer used to determine equality between items.</param>
        /// <returns>The modified replaceable collection.</returns>
        public static IList<T> Replace<T>(this IList<T> replaceableCollection, IList<T> newCollection, VennComparer<T> comparer)
        {
            if (newCollection == null)
                throw new ArgumentNullException(nameof(newCollection));

            foreach (var newItem in newCollection)
            {
                replaceableCollection.Replace(newItem, comparer);
            }

            return replaceableCollection;
        }

        /// <summary>
        /// Replaces items in the collection with a new item based on the provided comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to be modified.</param>
        /// <param name="newItem">The new item to replace existing items.</param>
        /// <param name="comparer">The comparer used to determine equality between items.</param>
        public static void Replace<T>(this IList<T> collection, T newItem, VennComparer<T> comparer)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (newItem == null)
                throw new ArgumentNullException(nameof(newItem));

            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            foreach (var oldItem in collection)
            {
                collection.ReplaceItem(oldItem, newItem, comparer);
            }
        }

        /// <summary>
        /// Replaces an old item with a new item in the collection based on the provided comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to be modified.</param>
        /// <param name="oldItem">The item to be replaced.</param>
        /// <param name="newItem">The new item.</param>
        /// <param name="comparer">The comparer used to determine equality between items.</param>
        /// <returns>True if the replacement is successful; otherwise, false.</returns>
        private static bool ReplaceItem<T>(this IList<T> collection, T oldItem, T newItem, VennComparer<T> comparer)
        {
            if (comparer.Equals(oldItem, newItem))
            {
                try
                {
                    int index = collection.IndexOf(oldItem);
                    if (index != -1)
                    {
                        collection[index] = newItem;
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle the exception as needed.
                    throw;
                }
            }

            return false;
        }

        #endregion

        #region Add Methods

        /// <summary>
        /// Adds items from the new collection to the target collection based on the provided comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collections.</typeparam>
        /// <param name="replaceableCollection">The collection to be modified.</param>
        /// <param name="newCollection">The collection containing new items.</param>
        /// <param name="comparer">The comparer used to determine equality between items.</param>
        /// <returns>The modified replaceable collection.</returns>
        public static IList<T> AddRange<T>(this IList<T> replaceableCollection, IList<T> newCollection, VennComparer<T> comparer)
        {
            if (replaceableCollection == null)
                throw new ArgumentNullException(nameof(replaceableCollection));

            if (newCollection == null)
                throw new ArgumentNullException(nameof(newCollection));

            foreach (var newItem in newCollection)
            {
                replaceableCollection.Add(newItem, comparer);
            }

            return replaceableCollection;
        }

        /// <summary>
        /// Adds a new item to the collection based on the provided comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to be modified.</param>
        /// <param name="newItem">The new item to be added.</param>
        /// <param name="comparer">The comparer used to determine equality between items.</param>
        public static void Add<T>(this IList<T> collection, T newItem, VennComparer<T> comparer)
        {
            if (!collection.Any())
            {
                collection.Add(newItem);
                return;
            }

            foreach (var oldItem in collection)
            {
                collection.AddItem(newItem, oldItem, comparer);
            }
        }

        /// <summary>
        /// Adds a new item to the collection based on the provided comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to be modified.</param>
        /// <param name="newItem">The new item to be added.</param>
        /// <param name="oldItem">The existing item in the collection.</param>
        /// <param name="comparer">The comparer used to determine equality between items.</param>
        private static void AddItem<T>(this IList<T> collection, T newItem, T oldItem, VennComparer<T> comparer)
        {
            if (oldItem != null && comparer != null && comparer.Equals(oldItem, newItem))
            {
                collection.Add(newItem);
            }
            else
            {
                collection.Add(newItem);
            }
        }

        #endregion

        #region ReplaceOrAdd Methods

        /// <summary>
        /// Replaces or adds items from the new collection to the target collection based on the provided predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collections.</typeparam>
        /// <param name="replaceableCollection">The collection to be modified.</param>
        /// <param name="newCollection">The collection containing new items.</param>
        /// <param name="replacePredicate">The condition that items must meet to be replaced in the replaceable collection.</param>
        /// <returns>The modified replaceable collection.</returns>
        [Obsolete("Use the Replace method with a comparer instead.")]
        public static IList<T> ReplaceOrAdd<T>(this IList<T> replaceableCollection, IList<T> newCollection, VennComparer<T> comparer)
        {
            replaceableCollection.Replace(newCollection, comparer);

            foreach (var newItem in newCollection)
            {
                if (replaceableCollection.IndexOf(newItem) != -1)
                {
                    replaceableCollection.Add(newItem);
                }
            }

            return replaceableCollection;
        }

        #endregion

        #region Slice Methods

        /// <summary>
        /// Retrieves a portion of the collection based on the specified start, end, step, and taken parameters.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to be sliced.</param>
        /// <param name="start">The starting index for slicing.</param>
        /// <param name="end">The ending index for slicing.</param>
        /// <param name="step">The step size for slicing.</param>
        /// <param name="taken">The number of elements to take during each step.</param>
        /// <returns>A sliced collection.</returns>
        public static IEnumerable<T> Slice<T>(this IList<T> collection, int start = 0, int end = 1, int step = 1, int taken = 1)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            List<T> result = new List<T>();

            for (int i = start; i < end; i += step)
            {
                yield return (T)collection.Skip(i).Take(taken);
            }
        }

        /// <summary>
        /// Retrieves a portion of the collection based on the specified start and end parameters.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to be sliced.</param>
        /// <param name="start">The starting index for slicing.</param>
        /// <param name="end">The ending index for slicing.</param>
        /// <returns>A sliced collection.</returns>
        public static IEnumerable<T> Slice<T>(this IList<T> collection, int start = 0, int end = 1)
        {
            return Slice<T>(collection, start, end, 1, 1);
        }

        /// <summary>
        /// Retrieves a portion of the collection based on the specified start parameter.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to be sliced.</param>
        /// <param name="start">The starting index for slicing.</param>
        /// <returns>A sliced collection.</returns>
        public static IEnumerable<T> Slice<T>(this IList<T> collection, int start = 0)
        {
            return Slice<T>(collection, start, collection.Count);
        }

        #endregion
    }
}
