using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Venn.Base.Bindables;
using Venn.Base.Interfaces;

namespace Venn.Extensions
{
    public static class NotifyPropertyChangedWrapperExtensions
    {
        #region WhenAny

        // Extension method to create an observable for changes in properties of NotifyPropertyChangedWrapper<T> objects
        public static IObservable<TProperty> WhenAny<T, TProperty>(
            this NotifyPropertyChangedWrapper<T> source,
            Expression<Func<T, TProperty>> propertySelector)
        {
            // Check for null parameters
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (propertySelector == null)
                throw new ArgumentNullException(nameof(propertySelector));

            var isSuspendable = source is ISuspendableNotifyPropertyChanged;
            var suspendable = isSuspendable ? source  as ISuspendableNotifyPropertyChanged : null;

            return source.Observable
                .Where(_ => source.Value != null)
                // Filter out events based on suspension status
                .Where(_ => !isSuspendable || !suspendable.IsSuspended)
                .Select(_ => propertySelector.Compile()(source.Value));
        }

        // Extension method to create an observable for changes in the underlying value of NotifyPropertyChangedWrapper<T> objects
        public static IObservable<T> WhenAny<T>(this NotifyPropertyChangedWrapper<T> source)
        {
            // Check for null parameters
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return CreateObservable(source);
        }

        #endregion

        #region WhenAnyValue

        // Extension method to create an observable for changes in properties of NotifyPropertyChangedWrapper<T> objects
        public static IObservable<TProperty> WhenAnyValue<T, TProperty>(
            this NotifyPropertyChangedWrapper<T> source,
            Expression<Func<T, TProperty>> propertySelector)
        {
            // Check for null parameters
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (propertySelector == null)
                throw new ArgumentNullException(nameof(propertySelector));

            var propertyName = propertySelector.GetPropertyName();

            return source.Observable
                .Select(_ => propertySelector.Compile()(source.Value))
                .StartWith(propertySelector.Compile()(source.Value))
                .Publish()
                .RefCount()
                .Where(_ => !(source is ISuspendableNotifyPropertyChanged suspendable) || !suspendable.IsSuspended);
        }

        // Extension method to create an observable for changes in the underlying value of NotifyPropertyChangedWrapper<T> objects
        public static IObservable<T> WhenAnyValue<T>(this NotifyPropertyChangedWrapper<T> source)
        {
            // Check for null parameters
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Observable
                .Select(_ => source.Value)
                .StartWith(source.Value)
                .Publish()
                .RefCount()
                .Where(_ => !(source is ISuspendableNotifyPropertyChanged suspendable) || !suspendable.IsSuspended);
        }

        #endregion

        // Overloaded helper method for CreateObservable for WhenAny<T>(NotifyPropertyChangedWrapper<T> source)
        private static IObservable<T> CreateObservable<T>(NotifyPropertyChangedWrapper<T> source)
        {
            var isSuspendable = source is ISuspendableNotifyPropertyChanged;
            var suspendable = isSuspendable ? source as ISuspendableNotifyPropertyChanged : null;

            return source.Observable
                .Where(_ => !isSuspendable || !suspendable.IsSuspended)
                .Where(_ => source.Value != null)
                .Select(_ => source.Value);
        }
    }
}
