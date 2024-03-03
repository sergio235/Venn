using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venn.Base;

namespace Venn.Navigation
{
    public interface INavigationService
    {
        void NavigateTo<TViewModel>() where TViewModel : BaseViewModel;
    }
}
