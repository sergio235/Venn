using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;
using Venn.PropertyChanges;

namespace Venn.Extensions
{
    public static class NotifyPropertyChangedExtensions
    {
        public static IObservable<TProperty> WhenAny<T, TProperty>(
            this T source,
            Expression<Func<T, TProperty>> propertySelector)
            where T : INotifyPropertyChanged
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (propertySelector == null)
                throw new ArgumentNullException(nameof(propertySelector));

            var propertyName = propertySelector.GetPropertyName();

            return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                h => source.PropertyChanged += h,
                h => source.PropertyChanged -= h)
                .Where(e => e.EventArgs.PropertyName == propertyName)
                .Select(_ => propertySelector.Compile()(source));
        }

        public static IObservable<TProperty> WhenAny<T, TProperty>(
            this NotifyPropertyChangedWrapper<T> source,
            Expression<Func<T, TProperty>> propertySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (propertySelector == null)
                throw new ArgumentNullException(nameof(propertySelector));

            var propertyName = propertySelector.GetPropertyName();

            return source.Observable
                .Where(_ => source.Value != null)
                .Select(_ => propertySelector.Compile()(source.Value));
        }

        public static IObservable<T> WhenAny<T>(this NotifyPropertyChangedWrapper<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Observable
                .Where(_ => source.Value != null)
                .Select(_ => source.Value);
        }
    }
}