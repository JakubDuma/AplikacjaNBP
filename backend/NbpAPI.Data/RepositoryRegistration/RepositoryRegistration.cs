using Microsoft.Extensions.DependencyInjection;
using NbpAPI.Data.Interface;
using NbpAPI.Data.Repository;

namespace NbpAPI.Data.RepositoryRegistration
{
    public static class RepositoryRegistration
    {
        public static void AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IRateRepository, RateRepository>();
        }
    }
}
