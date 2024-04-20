using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Venn.Navigation
{
    public interface INavigationService
    {
        void RegisterViewModelMapping(Type viewType, Type viewModelType);

        Task NavigateToAsync<TView>() where TView : Page;

        Task NavigateToAsync<TView>(object parameter) where TView : Page;

        Task NavigateBackAsync();
    }
}
