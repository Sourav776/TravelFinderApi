namespace TravelFinderApi.Configurations
{
    public static class StartupExtender
    {
        public static void AddSystemConfigurations(this IServiceCollection services)
        {
            services
               .AddSwaggerGen()
               .AddEndpointsApiExplorer()
               .AddHttpContextAccessor()
               .AddCors()
               .AddControllers()
               .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
               .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
        }
    }
}
