using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Venn
{
    public static class Venn
    {
        public static IEnumerable<T> Union<T>(IEnumerable<T> collectionA, IEnumerable<T> collectionB, IEqualityComparer<T> comparer = null)
        {
            if (comparer == null)
            {
                return collectionA.Union(collectionB);
            }
            return collectionA.Union(collectionB, comparer);
        }

        public static IEnumerable<T> Intersection<T>(IEnumerable<T> collectionA, IEnumerable<T> collectionB, IEqualityComparer<T> comparer = null)
        {
            if (comparer == null)
            {
                return collectionA.Intersect(collectionB);
            }
            return collectionA.Intersect(collectionB, comparer);
        }

        public static IEnumerable<T> SymmetricExceptWith<T>(IEnumerable<T> collectionA, IEnumerable<T> collectionB, IEqualityComparer<T> comparer = null)
        {
            IEnumerable<T> union;
            IEnumerable<T> intersection;
            if(comparer == null)
            {
                union = collectionA.Union(collectionB);
                intersection = collectionA.Intersect(collectionB);



            }
            return collectionA.Intersect(collectionB, comparer);
        }
    }
}
