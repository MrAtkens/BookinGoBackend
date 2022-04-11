using Booking.Models;
using Booking.Models.Business;
using Booking.Models.Users.User;
using Microsoft.EntityFrameworkCore;

namespace Booking.DataAccess
{
    public sealed class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
            //Database.Migrate();
        }

        #region DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Subscribes> Subscribes { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<BusinessImage> BusinessImages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }        

        public DbSet<ResetPasswordOperation> ResetPasswordOperations { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var superAdmin = new Admin
            {
                Id = Guid.Parse("9a8e2f25-b0f1-4b47-9f2f-a9d6e43b24ec"),
                Login = "dev",
                PasswordHash = "123456",
                Role = AdminRole.SuperAdmin
            };
            modelBuilder.Entity<Admin>().HasData(superAdmin);

            var user = new User
            {
                Id = Guid.Parse("25173041-7745-485e-a025-440c53de55f5"),
                Email = "test@mail.ru",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                FullName = "TestUser",
                Phone = "+7999999999",
                BirthDate = DateTime.Now,
                IsConfirmed = true,
                Rating = 0,
            };

            modelBuilder.Entity<User>().HasData(user);
            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(u => u.Slug)
                .IsUnique();
        }
    }

}
