using System.ComponentModel.DataAnnotations;

namespace Booking.Contracts.Parameters.Admin
{
    public class AdminAuthenticationParameter
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }

}
