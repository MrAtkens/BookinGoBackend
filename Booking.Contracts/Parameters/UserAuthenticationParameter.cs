using System.ComponentModel.DataAnnotations;

namespace Booking.Contracts.Parameters
{
    public class UserAuthenticationParameter
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

}
