using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Venn.PropertyChanges
{
    /// <summary>
    /// Wraps an object that does not implement INotifyPropertyChanged and notifies about property changes.
    /// </summary>
    /// <typeparam name="T">Type of the object to wrap.</typeparam>
    public class NotifyPropertyChangedWrapper<T> : INotifyPropertyChanged
    {
        private T _value;
        private readonly BehaviorSubject<T> _valueChanged;

        /// <summary>
        /// Initializes a new instance of the NotifyPropertyChangedWrapper class.
        /// </summary>
        /// <param name="initialValue">Initial value for the wrapped object.</param>
        public NotifyPropertyChangedWrapper(T initialValue)
        {
            _value = initialValue;
            _valueChanged = new BehaviorSubject<T>(_value);
        }

        /// <summary>
        /// Gets an observable that emits events whenever the property of the wrapped object changes.
        /// </summary>
        internal IObservable<T> Observable => _valueChanged.AsObservable();

        /// <summary>
        /// Gets or sets the wrapped object's value.
        /// </summary>
        public T Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value))
                {
                    return;
                }

                _value = value;
                OnPropertyChanged(nameof(Value));
                _valueChanged.OnNext(_value);
            }
        }

        /// <summary>
        /// Event that is triggered when a property of the wrapped object changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
