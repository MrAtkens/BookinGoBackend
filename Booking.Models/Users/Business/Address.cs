
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Models.Business
{
    public class Address : Entity
    {
        public string AddressName { get; set; }
        public double AddressLat { get; set; }
        public double AddressLng { get; set; }
        [ForeignKey("BusinessId")]
        public Guid BusinessId { get; set; }
    }
}
