using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Booking.DataAccess
{
    public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.core.json")
                .Build();

            var connectionString = config.GetConnectionString("SqlConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationContext(optionsBuilder.Options);
        }
    }

}
