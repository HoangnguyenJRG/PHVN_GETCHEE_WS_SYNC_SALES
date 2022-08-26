using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PHVN_WS_CORE.SERVICES.Braze;
using PHVN_WS_CORE.SERVICES.GetChee;
using PHVN_WS_CORE.SERVICES.DbAccess;

namespace PHVN_WS_CORE.SERVICES
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ISqlDataAccess, SqlDataAccess>();            
            services.AddSingleton< IGetCheeService,GetCheeService>();



        }
    }
}
