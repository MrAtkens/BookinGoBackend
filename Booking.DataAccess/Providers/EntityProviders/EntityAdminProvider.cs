using Booking.Contracts.Responses;
using Booking.DataAccess.Providers.Abstract;
using Booking.Models;

namespace Booking.DataAccess.Providers.EntityProviders
{
    public class EntityAdminProvider : EntityProvider<ApplicationContext, Admin, Guid>, IAdminProvider
    {
        public EntityAdminProvider(ApplicationContext context) : base(context)
        {

        }
        public async Task<Admin> GetByLogin(string login)
        {
            return await FirstOrDefault(x => x.Login == login) ?? throw new ArgumentException();
        }

        public async Task<List<GetAdminResponse>> GetAllResponses()
        {
            var admins = await Get(x => x.Role == AdminRole.Moderator);
            var adminResponses = admins.Select(admin => new GetAdminResponse
            {
                Id = admin.Id,
                Login = admin.Login,
                Role = admin.Role.ToString()
            }).ToList();
            return adminResponses;
        }
    }

}
