using Squib.UserService.API.config;
using Squib.UserService.API.Repository;
using Squib.UserService.API.Service;

namespace Squib.UserService.API
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSquibUserService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConnectionString>(configuration.GetSection("ConnectionStrings"));
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IUSER_Service, UserServi>();
            services.AddScoped<IORDER_Service,OrderServi>();
            services.AddScoped<IOrderRepo,OrderRepo>();
            // services.AddSingleton<IHubContext<TrackingHub>, HubContext<TrackingHub>>();
            return services;
        }
    }
}

