using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Models
{
    public class Subscribes : Entity
    {
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        [ForeignKey("BusinessId")]
        public Guid BusinessId { get; set; }
    }
}
