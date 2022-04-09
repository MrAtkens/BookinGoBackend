using Booking.Contracts.Dtos;
using Booking.Contracts.Options;
using Booking.Contracts.Parameters;
using Booking.Contracts.Responses;
using Booking.DataAccess.Providers.Abstract;
using Booking.Models.Users.User;
using Booking.Services;
using Booking.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace BookingApp.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/client/user/identity/[action]")]
    public class UserIdentityController : ControllerBase
    {
        private readonly UserAuthenticationService _authenticationService;
        private readonly IOptions<EmailSenderOptions> _emailSenderOptions;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IResetPasswordOperationProvider _operationProvider;
        private readonly IUserProvider _userProvider;
  
        private readonly UserIdentityUrlsOptions _userIdentityUrlsOptions;

        public UserIdentityController(UserAuthenticationService authenticationService, IOptions<EmailSenderOptions> emailSenderOptions, IEmailSenderService emailSenderService, IUserProvider userProvider,
            IOptions<UserIdentityUrlsOptions> userIdentityUrlsOptions)
        {
            _authenticationService = authenticationService;
            _emailSenderService = emailSenderService;
            _emailSenderOptions = emailSenderOptions;
            _userProvider = userProvider;
            _userIdentityUrlsOptions = userIdentityUrlsOptions.Value;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] UserAuthenticationParameter parameter)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("All parameters are required");
                }

                var token = await _authenticationService.Authenticate(parameter.Email, parameter.Password);

                if (string.IsNullOrWhiteSpace(token))
                {
                    return NotFound("No such user profile");
                }

                var user = await _userProvider.FirstOrDefault(x => x.Email.Equals(parameter.Email));
                if (user.IsConfirmed)
                {
                    return Ok(new UserAuthenticationResponse
                    {
                        Token = token
                    });
                }
                else
                {
                    return Forbid();
                }
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] RegisterUserParameter registerUser)
        {
            string token;
            try
            {
                token = await _authenticationService.Register(new UserRegistrationDto
                {
                    FullName = registerUser.FullName,
                    Phone = registerUser.Phone,
                    Email = registerUser.Email,
                    BirthDate = registerUser.BirthDate,
                    Password = registerUser.Password,
                });
            }
            catch (ArgumentException e)
            {
                return Unauthorized(e.Message);
            }
            var registeredUser = await _userProvider.GetByEmailOrPhone(registerUser.Email);

            var result = await _emailSenderService.SendEmail(new EmailDto
            {
                FromName = _emailSenderOptions.Value.Name,
                FromAddress = _emailSenderOptions.Value.Login,
                FromPassword = _emailSenderOptions.Value.Password,
                ToAddress = registerUser.Email,
                ToName = registerUser.FullName,
                Subject = "Подтверждение аккаунта",
                Text = _userIdentityUrlsOptions.AccountConfirmation + "?userId=" + registeredUser.Id
            });

            return result != "Ok" ? StatusCode(500, result) : Ok();
        }

        [HttpGet]
        public async Task<IActionResult> PasswordReset([Required] string email)
        {
            try
            {
                var user = await _userProvider.GetByEmailOrPhone(email) ?? throw new ArgumentException("No such user");
                var operation = new ResetPasswordOperation
                {
                    UserId = user.Id
                };
                await _operationProvider.Add(operation);

                return Ok(operation.Id);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmEmail(Guid userId)
        {
            try
            {
                var token = await _authenticationService.AuthenticateConfirmUser(userId);
                var user = await _userProvider.FirstOrDefault(x => x.Id.Equals(userId));
                var users = await _userProvider.Get(x => x.Email.Equals(user.Email) && !x.IsConfirmed);
                foreach (var userRemove in users)
                {
                    await _userProvider.Remove(userRemove);
                }
                return Ok(token);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostPasswordReset([FromBody] PostPasswordReset parameter)
        {
            var operation = await _operationProvider.GetById(parameter.OperationId);
            var user = await _userProvider.GetById(operation.UserId);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(parameter.Password);
            await _userProvider.Edit(user);
            return Ok();
        }

    }

}
