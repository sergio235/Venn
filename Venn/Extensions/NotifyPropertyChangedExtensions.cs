using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;
using Venn.Base.Interfaces;
using Venn.PropertyChanges;

namespace Venn.Extensions
{
    public static class NotifyPropertyChangedExtensions
    {

        #region WhenAny
        // Extension method to create an observable for changes in properties of objects implementing INotifyPropertyChanged
        public static IObservable<TProperty> WhenAny<T, TProperty>(
            this T source,
            Expression<Func<T, TProperty>> propertySelector)
            where T : INotifyPropertyChanged
        {
            // Check for null parameters
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (propertySelector == null)
                throw new ArgumentNullException(nameof(propertySelector));

            return CreateObservable(source, propertySelector);
        }

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
            var suspendable = isSuspendable ? source as ISuspendableNotifyPropertyChanged : null;

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
        // Extension method to create an observable for changes in properties of objects implementing INotifyPropertyChanged
        public static IObservable<TProperty> WhenAnyValue<T, TProperty>(
            this T source,
            Expression<Func<T, TProperty>> propertySelector)
            where T : INotifyPropertyChanged
        {
            // Check for null parameters
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (propertySelector == null)
                throw new ArgumentNullException(nameof(propertySelector));

            var propertyName = propertySelector.GetPropertyName();

            return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    h => source.PropertyChanged += h,
                    h => source.PropertyChanged -= h)
                .Where(e => e.EventArgs.PropertyName == propertyName)
                .Select(_ => propertySelector.Compile()(source))
                .StartWith(propertySelector.Compile()(source))
                .Publish()
                .RefCount()
                .Where(_ => !(source is ISuspendableNotifyPropertyChanged suspendable) || !suspendable.IsSuspended);
        }

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

        // Method to suspend notifications for objects implementing ISuspendableNotifyPropertyChanged
        public static void SuspendNotifications(this ISuspendableNotifyPropertyChanged source)
        {
            // Check for null parameter
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            // Set the IsSuspended property to true to suspend notifications
            source.IsSuspended = true;
        }

        // Method to resume notifications for objects implementing ISuspendableNotifyPropertyChanged
        public static void ResumeNotifications(this ISuspendableNotifyPropertyChanged source)
        {
            // Check for null parameter
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            // Set the IsSuspended property to false to resume notifications
            source.IsSuspended = false;
        }

        // Helper method to create the observable for property changes
        private static IObservable<TProperty> CreateObservable<T, TProperty>(
            T source,
            Expression<Func<T, TProperty>> propertySelector)
            where T : INotifyPropertyChanged
        {
            var propertyName = propertySelector.GetPropertyName();
            var isSuspendable = source is ISuspendableNotifyPropertyChanged;
            var suspendable = isSuspendable ? source as ISuspendableNotifyPropertyChanged : null;

            return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    h => source.PropertyChanged += h,
                    h => source.PropertyChanged -= h)
                .Where(e => !isSuspendable || !suspendable.IsSuspended)
                .Where(e => e.EventArgs.PropertyName == propertyName)
                .Select(_ => propertySelector.Compile()(source));
        }

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
