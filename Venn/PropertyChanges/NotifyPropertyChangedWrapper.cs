using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Venn.Base.Interfaces;

namespace Venn.PropertyChanges
{
    /// <summary>
    /// Wraps an object that does not implement INotifyPropertyChanged and notifies about property changes.
    /// </summary>
    /// <typeparam name="T">Type of the object to wrap.</typeparam>
    public class NotifyPropertyChangedWrapper<T> : ISuspendableNotifyPropertyChanged
    {
        private T _value;
        private readonly BehaviorSubject<T> _valueChanged;
        private readonly object _lockObject = new object();  // Added for synchronization
        private bool _isSuspended;

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

                // Use a lock to ensure thread-safe updates
                lock (_lockObject)
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                    _valueChanged.OnNext(_value);
                }
            }
        }

        public bool IsSuspended
        {
            get => _isSuspended;
            set
            {
                if (_isSuspended != value)
                {
                    _isSuspended = value;
                    OnPropertyChanged(nameof(IsSuspended));
                }
            }
        }

        /// <summary>
        /// Event that is triggered when a property of the wrapped object changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            // Check if notifications are paused before invoking the event
            if (!_isSuspended)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Pauses notifications temporarily.
        /// </summary>
        public void Pause()
        {
            // Use a lock to ensure thread-safe modification
            lock (_lockObject)
            {
                _isSuspended = true;
            }
        }

        /// <summary>
        /// Resumes notifications.
        /// </summary>
        public void Resume()
        {
            // Use a lock to ensure thread-safe modification
            lock (_lockObject)
            {
                _isSuspended = false;
            }
        }
    }
}
