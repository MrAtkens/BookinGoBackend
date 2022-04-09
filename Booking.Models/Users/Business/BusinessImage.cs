
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Models.Business
{
    public class BusinessImage : Image
    {
        [ForeignKey("BusinessId")]
        public Guid BusinessId { get; set; }
    }
}
