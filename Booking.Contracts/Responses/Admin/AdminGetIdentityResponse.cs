namespace Booking.Contracts.Responses.Admin
{
    public class AdminGetIdentityResponse
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }
    }

}
