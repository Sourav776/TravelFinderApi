using TravelFinderApi.Helpers;
using TravelFinderApi.Services.Implementation;
using TravelFinderApi.Services.Interface;

namespace TravelFinderApi.Configurations
{
    public static class DependencyResolver
    {
        public static void AddDependencyResolver(this IServiceCollection services)
        {
            services.AddHttpClient<IDistrictService, DistrictService>();
            services.AddHttpClient<OpenMeteoClient>();

            services.AddScoped<IWeatherService, WeatherService>();
        }
    }
}
