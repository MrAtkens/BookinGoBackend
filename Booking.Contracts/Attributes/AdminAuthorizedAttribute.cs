using Booking.Models;
using Microsoft.AspNetCore.Authorization;


namespace Booking.Contracts.Attributes
{
    public sealed class AdminAuthorizedAttribute : AuthorizeAttribute
    {
        public AdminAuthorizedAttribute(params AdminRole[] roles)
        {
            Roles = String.Join(",", roles);
        }
        public AdminAuthorizedAttribute()
        {
            var roles = new AdminRole[] { AdminRole.Moderator, AdminRole.SuperAdmin };
            Roles = String.Join(",", roles);
        }
    }

}
