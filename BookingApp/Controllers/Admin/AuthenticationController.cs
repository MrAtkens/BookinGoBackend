using Booking.Contracts.Parameters.Admin;
using Booking.Contracts.Responses.Admin;
using Booking.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace BookingApp.Controllers.Admin
{

    [ApiExplorerSettings(GroupName = "v1-admin")]
    [Route("api/admin/identity")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AdminAuthenticationService _authenticationService;

        public AuthenticationController(AdminAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AdminAuthenticationParameter parameter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("All parameters are required");
            }

            var token = await _authenticationService.Authenticate(parameter.Login, parameter.Password);

            // if token is null, password incorrect
            if (string.IsNullOrWhiteSpace(token))
            {
                return NotFound("No such admin profile");
            }

            return Ok(new AdminAuthenticationResponse
            {
                Token = token
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            try
            {
                var data = await _authenticationService
                    .GetAdminByHeaders(Request.Headers[HeaderNames.Authorization].ToArray());

                return Ok(new AdminGetIdentityResponse
                {
                    Id = data.Id,
                    Login = data.Login,
                    Role = data.Role.ToString()
                });
            }
            catch (ArgumentException e)
            {
                return Unauthorized("Admin not found");
            }

        }

    }

}
