
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Models.Business
{
    public class Business : Entity
    {
        public string BusinessName { get; set; }
        public string Description { get; set; }
        public string PasswordHash { get; set; }
        public bool isConfirm { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime WorkDayStartTime { get; set; }
        public DateTime WorkDayEndTime { get; set; }
        [ForeignKey("CategoryId")]
        public Guid CategoryId { get; set; }
    }
}
