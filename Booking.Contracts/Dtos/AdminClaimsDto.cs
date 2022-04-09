using Booking.Models;

namespace Booking.Contracts.Dtos
{
    public class AdminClaimsDto
    {
        public string Login { get; set; }
        public AdminRole Role { get; set; }
    }

}
