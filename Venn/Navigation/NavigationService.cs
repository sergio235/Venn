using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Venn.Base.ViewModels;

namespace Venn.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly NavigationPage _navigationPage;
        private readonly Dictionary<Type, Type> _viewModelMapping = new Dictionary<Type, Type>();

        public NavigationService(NavigationPage navigationPage)
        {
            _navigationPage = navigationPage;
            HookBackButton();
        }

        public void RegisterViewModelMapping(Type viewType, Type viewModelType)
        {
            _viewModelMapping[viewType] = viewModelType;
        }

        public async Task NavigateToAsync<TView>() where TView : Page
        {
            var viewModelType = GetViewModelType(typeof(TView));
            var viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType);
            var page = (Page)Activator.CreateInstance(typeof(TView));
            page.BindingContext = viewModel;
            await _navigationPage.PushAsync(page);
        }

        public async Task NavigateToAsync<TView>(object parameter) where TView : Page
        {
            var viewModelType = GetViewModelType(typeof(TView));
            var viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType);
            var page = (Page)Activator.CreateInstance(typeof(TView), parameter);
            page.BindingContext = viewModel;
            await _navigationPage.PushAsync(page);
        }

        public async Task NavigateBackAsync()
        {
            await _navigationPage.PopAsync();
        }

        private Type GetViewModelType(Type viewType)
        {
            if (!_viewModelMapping.TryGetValue(viewType, out Type viewModelType))
            {
                throw new ArgumentException($"ViewModel not registered for View '{viewType.FullName}'");
            }

            return viewModelType;
        }

        private void HookBackButton()
        {
            //_navigationPage.Popped += async (sender, e) =>
            //{
            //    // Puedes agregar lógica adicional aquí si es necesario
            //};
        }
    }
}
