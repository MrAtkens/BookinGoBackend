using Booking.DataAccess.Providers.Abstract;
using Booking.Models.Users.User;

namespace Booking.DataAccess.Providers.EntityProviders
{
    public class EntityResetPasswordOperationProvider : EntityProvider<ApplicationContext, ResetPasswordOperation, Guid>, IResetPasswordOperationProvider
    {
        private readonly ApplicationContext _context;
        public EntityResetPasswordOperationProvider(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }

}
