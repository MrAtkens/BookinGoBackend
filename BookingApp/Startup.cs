using BookingApp.AppStart.Configures;
using BookingApp.AppStart.ConfigureServices;

namespace BookingApp
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.core.json")
                .Build();
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureServicesProvider.ConfigureServices(services, Configuration);
            ConfigureServicesAppServices.ConfigureServices(services, Configuration);
            ConfigureServicesBase.ConfigureServices(services, Configuration);
            ConfigureServicesSwagger.ConfigureServices(services, Configuration);

            ConfigureServicesCors.ConfigureServices(services, Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ConfigureCommon.Configure(app, env);
            ConfigureEndpoints.Configure(app, env);
        }

    }

}
