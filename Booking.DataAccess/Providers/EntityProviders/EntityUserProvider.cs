using Booking.Contracts.Dtos;
using Booking.DataAccess.Providers.Abstract;
using Booking.Models;

namespace Booking.DataAccess.Providers.EntityProviders
{
    public class EntityUserProvider : EntityProvider<ApplicationContext, User, Guid>, IUserProvider
    {
        private readonly ApplicationContext _context;

        public EntityUserProvider(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailOrPhone(string emailOrPhone)
        {
            var user = await Get(x =>
                x.Email.ToLower().Equals(emailOrPhone.ToLower()) ||
                x.Phone.ToLower().Equals(emailOrPhone.ToLower()));
             return user.FirstOrDefault() ?? throw new ArgumentException("User is not found");
        }

        public async Task Add(UserRegistrationDto user)
        {
            await Add(new User
            {
                FullName = user.FullName,
                Phone = user.Phone,
                PasswordHash = user.Password,
                BirthDate = user.BirthDate,
                Email = user.Email,
            });
        }
    }

}
