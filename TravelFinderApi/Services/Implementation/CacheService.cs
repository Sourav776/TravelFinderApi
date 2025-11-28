
using TravelFinderApi.Services.Interface;

namespace TravelFinderApi.Services.Implementation
{
    public class CacheService : IHostedService
    {
        private readonly IServiceProvider _provider;
        public CacheService(IServiceProvider provider)
        {
            _provider = provider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _provider.CreateScope();
            var districtService = scope.ServiceProvider.GetRequiredService<IDistrictService>();
            await districtService.CacheDistrictsAsync();
            var weatherService = scope.ServiceProvider.GetRequiredService<IWeatherService>();
            await weatherService.CacheTopDistrictsAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
