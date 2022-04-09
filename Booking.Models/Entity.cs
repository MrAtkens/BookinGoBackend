namespace Booking.Models
{
    public class Entity
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }

}
