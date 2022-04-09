
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Models
{
    public class Comment : Entity
    {
        [ForeignKey("BusinessId")]
        public Guid BusinessId { get; set; }
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public double Rating { get; set; }
    }
}
