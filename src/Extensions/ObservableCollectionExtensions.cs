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
