namespace Booking.Contracts.Parameters
{
    public class PostPasswordReset
    {
        public string Password { get; set; }
        public Guid OperationId { get; set; }
    }

}
