using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Venn.Comparers
{
    public class VennComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparerExp;

        public VennComparer(Func<T, T, bool> comparerExp)
        {
            if (comparerExp == null)
                throw new ArgumentNullException("comparerExp");

            _comparerExp = comparerExp;
        }

        public bool Equals(T x, T y)
        {
            return _comparerExp(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj.ToString().ToLower().GetHashCode();
        }
    }

}
