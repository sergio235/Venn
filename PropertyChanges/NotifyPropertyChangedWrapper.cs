using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Venn.PropertyChanges
{
    public class NotifyPropertyChangedWrapper<T> : INotifyPropertyChanged
    {
        private T _value;
        private readonly BehaviorSubject<T> _valueChanged;

        public NotifyPropertyChangedWrapper(T initialValue)
        {
            _value = initialValue;
            _valueChanged = new BehaviorSubject<T>(_value);
        }

        internal IObservable<T> Observable => _valueChanged.AsObservable();

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

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
