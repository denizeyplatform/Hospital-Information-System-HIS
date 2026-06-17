using Identity.PL.API.Common.Validator;

namespace Identity.PL.API.Configurations
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddAPI(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiKeySettings>(configuration.GetSection("ApiKeySettings"));
            services.AddSingleton<IApiKeyValidator, ApiKeyValidator>();
            return services;
        }
    }
}
