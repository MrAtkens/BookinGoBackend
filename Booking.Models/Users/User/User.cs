namespace Booking.Models
{
    public class User : Entity
    {

        public string Email { get; set; }
        public string FullName { get;set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public double Rating { get; set; } = 0;
        public DateTime BirthDate { get; set; }
        public bool IsConfirmed { get; set; } = false;
    }

}
