using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Venn.Base.Interfaces;
using Venn.PropertyChanges;

namespace Venn.Extensions
{
    public static class INotifyPropertyChangedExtensions
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

        #endregion

        #region WhenAnyAsync
        public static async Task<IObservable<TProperty>> WhenAnyAsync<T, TProperty>(
            this T source,
            Expression<Func<T, TProperty>> propertySelector,
            IScheduler observeOnScheduler = null)
            where T : INotifyPropertyChanged
        {
            // Check for null parameters
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (propertySelector == null)
                throw new ArgumentNullException(nameof(propertySelector));

            return await CreateObservableAsync(source, propertySelector, observeOnScheduler);
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

        #endregion

        #region WhenAnyValueAsync
        public static async Task<IObservable<TProperty>> WhenAnyValueAsync<T, TProperty>(
            this T source,
            Expression<Func<T, TProperty>> propertySelector,
            IScheduler observeOnScheduler = null)
            where T : INotifyPropertyChanged
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (propertySelector == null)
                throw new ArgumentNullException(nameof(propertySelector));

            var observable = CreateObservable(source, propertySelector, observeOnScheduler);
            return await Task.FromResult(observable);
        }
        #endregion

        // Helper method to create the observable for property changes
        private static IObservable<TProperty> CreateObservable<T, TProperty>(
            T source,
            Expression<Func<T, TProperty>> propertySelector)
            where T : INotifyPropertyChanged
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (propertySelector == null)
                throw new ArgumentNullException(nameof(propertySelector));

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

        private static IObservable<TProperty> CreateObservable<T, TProperty>(
            T source,
            Expression<Func<T, TProperty>> propertySelector,
            IScheduler observeOnScheduler = null)
            where T : INotifyPropertyChanged
        {
            var scheduler = observeOnScheduler ?? CurrentThreadScheduler.Instance;

            var observable = CreateObservable(source, propertySelector)
                .StartWith(propertySelector.Compile()(source))
                .ObserveOn(scheduler); // Observa los cambios en el hilo especificado o el actual

            return observable;
        }

        public static Task<IObservable<TProperty>> CreateObservableAsync<T, TProperty>(
            T source,
            Expression<Func<T, TProperty>> propertySelector,
            IScheduler observeOnScheduler = null)
            where T : INotifyPropertyChanged
        {
            var observable = CreateObservable(source, propertySelector, observeOnScheduler);

            return Task.FromResult(observable);
        }
    }
}
