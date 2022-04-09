using Booking.Contracts.Options;
using Booking.DataAccess;
using Booking.Services;
using Booking.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.AppStart.ConfigureServices
{
    public class ConfigureServicesAppServices
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            /* services.AddDbContext<ApplicationContext>(options =>
             options.UseInMemoryDatabase(configuration.GetConnectionString("InMemoryDb")));*/
            services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SqlConnection")));

            services.Configure<SecretOption>(configuration.GetSection("Secrets"));
            services.Configure<AwsS3Options>(configuration.GetSection("AwsS3"));
            services.Configure<EmailSenderOptions>(configuration.GetSection("EmailAccountCredentials"));
            services.Configure<SmtpClientOptions>(configuration.GetSection("SMTPClient"));
            services.Configure<UserIdentityUrlsOptions>(configuration.GetSection("UserIdentityUrls"));

            services.AddScoped<AwsS3FileUploadService>();
            services.AddScoped<IEmailSenderService, MailKitEmailSenderService>();
            services.AddScoped<UserAuthenticationService>();
            services.AddScoped<AdminAuthenticationService>();

            services.AddAuthentication();
            services.AddAuthorization();
        }
    }

}
