using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Models.Business
{
    public class Phone : Entity
    {
        public string PhoneNumber { get; set; }
        [ForeignKey("BusinessId")]
        public Guid BusinessId { get; set; }
    }
}
