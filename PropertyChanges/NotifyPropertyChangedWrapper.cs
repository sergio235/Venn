using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace Venn.PropertyChanges
{
    public class NotifyPropertyChangedWrapper<T> : INotifyPropertyChanged
    {
        private readonly T _wrappedObject;
        private readonly BehaviorSubject<Unit> _propertyChangedSubject = new BehaviorSubject<Unit>(Unit.Default);

        public NotifyPropertyChangedWrapper(T wrappedObject)
        {
            _wrappedObject = wrappedObject ?? throw new ArgumentNullException(nameof(wrappedObject));
        }

        public IObservable<Unit> PropertyChangedObservable => _propertyChangedSubject.AsObservable();

        public T WrappedObject => _wrappedObject;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            _propertyChangedSubject.OnNext(Unit.Default);
        }

        public static NotifyPropertyChangedWrapper<T> Wrap(T obj)
        {
            return new NotifyPropertyChangedWrapper<T>(obj);
        }
    }

}
