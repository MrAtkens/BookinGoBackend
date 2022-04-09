namespace Booking.Contracts.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public double Rating { get; set; }
        public DateTime BirthDate { get; set; }

    }

}
