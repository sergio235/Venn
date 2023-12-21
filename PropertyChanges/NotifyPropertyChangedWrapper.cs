using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace Venn.PropertyChanges
{
    /// <summary>
    /// Class that wraps an object not implementing INotifyPropertyChanged and notifies property changes.
    /// </summary>
    /// <typeparam name="T">Type of the object to wrap.</typeparam>
    public class NotifyPropertyChangedWrapper<T> : INotifyPropertyChanged
    {
        private readonly T _wrappedObject;
        private readonly BehaviorSubject<Unit> _propertyChangedSubject = new BehaviorSubject<Unit>(Unit.Default);

        /// <summary>
        /// Initializes a new instance of the NotifyPropertyChangedWrapper class.
        /// </summary>
        /// <param name="wrappedObject">Object to wrap.</param>
        public NotifyPropertyChangedWrapper(T wrappedObject)
        {
            _wrappedObject = wrappedObject ?? throw new ArgumentNullException(nameof(wrappedObject));
        }

        /// <summary>
        /// Gets an observable that emits events whenever a property of the wrapped object changes.
        /// </summary>
        public IObservable<Unit> PropertyChangedObservable => _propertyChangedSubject.AsObservable();

        /// <summary>
        /// Gets the wrapped object.
        /// </summary>
        public T WrappedObject => _wrappedObject;

        /// <summary>
        /// Event that is triggered when a property of the wrapped object changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            _propertyChangedSubject.OnNext(Unit.Default);
        }

        /// <summary>
        /// Static method to wrap an object and create an instance of NotifyPropertyChangedWrapper.
        /// </summary>
        /// <param name="obj">Object to wrap.</param>
        /// <returns>Instance of NotifyPropertyChangedWrapper wrapping the provided object.</returns>
        public static NotifyPropertyChangedWrapper<T> Wrap(T obj)
        {
            return new NotifyPropertyChangedWrapper<T>(obj);
        }
    }
}
