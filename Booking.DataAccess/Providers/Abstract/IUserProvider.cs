using Booking.Contracts.Dtos;
using Booking.Models;

namespace Booking.DataAccess.Providers.Abstract
{
    public interface IUserProvider : IProvider<User, Guid>
    {
        Task<User> GetByEmailOrPhone(string emailOrPhone);
        Task Add(UserRegistrationDto user);
    }

}
