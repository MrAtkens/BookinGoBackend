using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Models.Users.User
{
    public class ResetPasswordOperation : Entity
    {
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public DateTime CompletedDate { get; set; }
    }

}
