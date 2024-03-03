using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venn.Navigation;

namespace Venn
{
    public static class ServiceCollectionExtensions
    {
        public static void AddVenn(this IServiceCollection services)
        {
            services.AddSingleton<INavigationService, NavigationService>();
        }
    }
}
