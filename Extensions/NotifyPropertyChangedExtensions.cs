using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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

            var propertyName = GetPropertyName(propertySelector);

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

            var propertyName = GetPropertyName(propertySelector);

            return source.MyPropertyChangedObservable
                .Where(_ => source.Value != null)
                .Select(_ => propertySelector.Compile()(source.Value));
        }

        public static IObservable<T> WhenAny<T>(this NotifyPropertyChangedWrapper<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            //if (propertySelector == null)
            //    throw new ArgumentNullException(nameof(propertySelector));

            //var propertyName = GetPropertyName(propertySelector);

            return source.MyPropertyChangedObservable
                .Where(_ => source.Value != null);
        }

        private static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> propertySelector)
        {
            if (propertySelector.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }
            throw new ArgumentException("Invalid property expression");
        }
    }
}