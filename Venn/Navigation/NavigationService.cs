using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venn.Base;

namespace Venn.Navigation
{
    public class NavigationService : INavigationService
    {
        private Dictionary<Type, Type> registrations;

        public NavigationService(IServiceProvider serviceProvider)
        {
            registrations = new Dictionary<Type, Type>();
        }

        public void NavigateTo<TViewModel>() where TViewModel : BaseViewModel
        {
            var viewModelType = typeof(TViewModel);
            var viewModelName = viewModelType.Name;

            var viewName = viewModelName.Replace("ViewModel", "Page");
            var viewType = Type.GetType($"{viewModelType.Namespace}.Views.{viewName}");

            if (viewType == null)
            {
                throw new InvalidOperationException($"View for {viewModelName} not found.");
            }

            var shellContent = new ShellContent
            {
                Content = (Page)Activator.CreateInstance(viewType)
            };

            Application.Current.MainPage = new Shell
            {
                CurrentItem = new ShellItem
                {
                    Items = { shellContent }
                }
            };

            // Optional: You can register the ViewModel and View relationship using ViewModelLocator here.
            // ViewModelLocator.Register(typeof(TViewModel), viewType);
        }

        public void RegisterForNavigation<TView, TViewModel>(Type viewType, Type viewModelType) 
            where TView : Page 
            where TViewModel : BaseViewModel
        {

        }
    }
}
