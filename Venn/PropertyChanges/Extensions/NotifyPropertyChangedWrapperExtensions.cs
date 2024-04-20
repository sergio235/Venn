using System;
using Venn.Base.Bindables;

namespace Venn.PropertyChanges.Extensions
{
    public static class NotifyPropertyChangedWrapperExtensions
    {
        public static NotifyPropertyChangedWrapper<T> Wrap<T>(this T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            return new NotifyPropertyChangedWrapper<T>(obj);
        }
    }
}
