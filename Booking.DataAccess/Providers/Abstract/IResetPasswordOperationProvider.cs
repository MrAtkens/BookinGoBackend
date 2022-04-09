using Booking.Models.Users.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.DataAccess.Providers.Abstract
{
    public interface IResetPasswordOperationProvider : IProvider<ResetPasswordOperation, Guid>
    {

    }

}
