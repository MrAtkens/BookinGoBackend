namespace Booking.Contracts.Parameters
{
    public class RegisterUserParameter
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }

    }

}
