using Booking.Contracts.Responses;
using Booking.Models;

namespace Booking.DataAccess.Providers.Abstract
{
    public interface IAdminProvider : IProvider<Admin, Guid>
    {
        Task<Admin> GetByLogin(string login);
        Task<List<GetAdminResponse>> GetAllResponses();
    }

}
