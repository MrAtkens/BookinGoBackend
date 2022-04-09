using Booking.DataAccess.Providers.Abstract;
using Booking.DataAccess.Providers.EntityProviders;

namespace BookingApp.AppStart.ConfigureServices
{
    public class ConfigureServicesProvider
    {
        /// <summary>
        /// ConfigureServices Providers
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserProvider, EntityUserProvider>();
            services.AddScoped<ICategoryProvider, EntityCategoryProvider>();
            services.AddScoped<IResetPasswordOperationProvider, EntityResetPasswordOperationProvider>();
            services.AddScoped<IAdminProvider, EntityAdminProvider>();
        }
    }

}
