namespace Booking.Contracts.Responses
{
    public class GetAdminResponse
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }
    }

}
