using Microsoft.Extensions.DependencyInjection;
using NbpAPI.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NbpAPI.Services.ServiceRegistration
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<CurrencyService>();
        }
    }
}
