using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Venn.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void ObservePropertyChanges<T>(
            this ObservableCollection<T> collection,
            Action<T> onPropertyChange,
            NotifyCollectionChangedAction actionsToObserve = NotifyCollectionChangedAction.Add | NotifyCollectionChangedAction.Remove | NotifyCollectionChangedAction.Replace | NotifyCollectionChangedAction.Reset,
            params Expression<Func<T, object>>[] propertySelectors)
            where T : INotifyPropertyChanged
        {
            collection.CollectionChanged += (sender, e) =>
            {
                if ((actionsToObserve & e.Action) != 0)
                {
                    if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
                    {
                        foreach (T item in e.NewItems)
                        {
                            AttachPropertyChangedHandlers(item, propertySelectors, onPropertyChange);
                        }
                    }
                    else if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
                    {
                        foreach (T item in e.OldItems)
                        {
                            DetachPropertyChangedHandlers(item, propertySelectors, onPropertyChange);
                        }
                    }
                    else if (e.Action == NotifyCollectionChangedAction.Reset)
                    {
                        if (actionsToObserve.HasFlag(NotifyCollectionChangedAction.Reset))
                        {
                            foreach (T item in collection)
                            {
                                AttachPropertyChangedHandlers(item, propertySelectors, onPropertyChange);
                            }
                        }
                    }
                }
            };
        }

        public static int FindIndex<T>(this ObservableCollection<T> ts, Predicate<T> match)
        {
            return ts.FindIndex(0, ts.Count, match);
        }

        public static int FindIndex<T>(this ObservableCollection<T> ts, int startIndex, Predicate<T> match)
        {
            return ts.FindIndex(startIndex, ts.Count, match);
        }

        public static int FindIndex<T>(this ObservableCollection<T> ts, int startIndex, int count, Predicate<T> match)
        {
            if (startIndex < 0) startIndex = 0;
            if (count > ts.Count) count = ts.Count;

            for (int i = startIndex; i < count; i++)
            {
                if (match(ts[i])) return i;
            }

            return -1;
        }

        private static void AttachPropertyChangedHandlers<T>(T item, Expression<Func<T, object>>[] propertySelectors, Action<T> onPropertyChange) where T : INotifyPropertyChanged
        {
            foreach (var propertySelector in propertySelectors)
            {
                var propertyName = propertySelector.GetPropertyName();
                item.PropertyChanged += (s, args) =>
                {
                    if (args.PropertyName == propertyName)
                    {
                        onPropertyChange(item);
                    }
                };
            }
        }

        private static void DetachPropertyChangedHandlers<T>(T item, Expression<Func<T, object>>[] propertySelectors, Action<T> onPropertyChange) where T : INotifyPropertyChanged
        {
            foreach (var propertySelector in propertySelectors)
            {
                var propertyName = propertySelector.GetPropertyName();
                item.PropertyChanged -= (s, args) =>
                {
                    if (args.PropertyName == propertyName)
                    {
                        onPropertyChange(item);
                    }
                };
            }
        }
    }
}
