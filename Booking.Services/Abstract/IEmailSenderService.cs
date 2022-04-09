
using Booking.Contracts.Dtos;

namespace Booking.Services.Abstract
{
     public interface IEmailSenderService
    {
        Task<string> SendEmail(EmailDto data);
    }
}
