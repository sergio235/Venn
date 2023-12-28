using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace Venn.Extensions
{
    public static class DisposableExtensions
    {
        public static IDisposable DisposeWith(this IDisposable disposable, CompositeDisposable compositeDisposable)
        {
            if (disposable == null)
            {
                throw new ArgumentNullException(nameof(disposable));
            }

            if (compositeDisposable == null)
            {
                throw new ArgumentNullException(nameof(compositeDisposable));
            }

            compositeDisposable.Add(disposable);

            return disposable;
        }
    }
}
