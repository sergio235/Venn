namespace Venn.Comparers
{
    /// <summary>
    /// Custom comparer class implementing IEqualityComparer for Venn diagrams.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    public class VennComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparerExp;

        /// <summary>
        /// Initializes a new instance of the VennComparer class with the specified comparison expression.
        /// </summary>
        /// <param name="comparerExp">The comparison expression defining the equality between objects of type T.</param>
        /// <exception cref="ArgumentNullException">Thrown when comparerExp is null.</exception>
        public VennComparer(Func<T, T, bool> comparerExp)
        {
            _comparerExp = comparerExp ?? throw new ArgumentNullException(nameof(comparerExp));
        }

        /// <summary>
        /// Determines whether two objects are equal based on the comparison expression.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if the objects are equal according to the comparison expression; otherwise, false.</returns>
        public bool Equals(T x, T y)
        {
            if (x == null || y == null)
                return false;

            return _comparerExp(x, y);
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The object for which to get a hash code.</param>
        /// <returns>A hash code for the specified object.</returns>
        public int GetHashCode(T obj)
        {
            return EqualityComparer<T>.Default.GetHashCode(obj);
        }
    }
}
