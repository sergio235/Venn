using Venn.Comparers;

namespace Venn.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Union<T>(this IEnumerable<T> collectionA, IEnumerable<T> collectionB, VennComparer<T> comparer = null)
        {
            if (comparer == null)
            {
                return collectionA.Union(collectionB);
            }
            return collectionA.Union(collectionB, comparer);
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<T> collectionA, IEnumerable<T> collectionB, VennComparer<T> comparer = null)
        {
            if (comparer == null)
            {
                return collectionA.Intersect(collectionB);
            }
            return collectionA.Intersect(collectionB, comparer);
        }

        public static IEnumerable<T> SymmetricExceptWith<T>(this IEnumerable<T> collectionA, IEnumerable<T> collectionB, VennComparer<T> comparer = null)
        {
            IEnumerable<T> union;
            IEnumerable<T> intersection;
            if (comparer == null)
            {
                union = collectionA.Union(collectionB);
                intersection = collectionA.Intersect(collectionB);
            }
            else
            {
                union = collectionA.Union(collectionB, comparer);
                intersection = collectionA.Intersect(collectionB, comparer);
            }

            return union.Except(intersection);
        }
    }
}
