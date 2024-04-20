using System.ComponentModel;

namespace Venn.Utils
{
    public sealed class NotifyTaskCompletion<TResult> : INotifyPropertyChanged
    {
        private readonly Task<TResult> _task;

        public NotifyTaskCompletion(Task<TResult> task)
        {
            _task = task ?? throw new ArgumentNullException(nameof(task));
            TaskCompletion = WatchTaskAsync();
        }

        private async Task WatchTaskAsync()
        {
            try
            {
                await _task;
            }
            catch
            {
                // Capturar excepciones silenciosamente
            }

            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(IsCompleted));
            OnPropertyChanged(nameof(IsNotCompleted));

            if (_task.IsCanceled)
                OnPropertyChanged(nameof(IsCanceled));
            else if (_task.IsFaulted)
            {
                OnPropertyChanged(nameof(IsFaulted));
                OnPropertyChanged(nameof(Exception));
                OnPropertyChanged(nameof(InnerException));
                OnPropertyChanged(nameof(ErrorMessage));
            }
            else
            {
                OnPropertyChanged(nameof(IsSuccessfullyCompleted));
                OnPropertyChanged(nameof(Result));
            }
        }

        public Task TaskCompletion { get; }

        public TResult Result => _task.Status == TaskStatus.RanToCompletion ? _task.Result : default;

        public TaskStatus Status => _task.Status;

        public bool IsCompleted => _task.IsCompleted;

        public bool IsNotCompleted => !_task.IsCompleted;

        public bool IsSuccessfullyCompleted => _task.Status == TaskStatus.RanToCompletion;

        public bool IsCanceled => _task.IsCanceled;

        public bool IsFaulted => _task.IsFaulted;

        public AggregateException Exception => _task.Exception;

        public Exception InnerException => Exception?.InnerException;

        public string ErrorMessage => InnerException?.Message;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
