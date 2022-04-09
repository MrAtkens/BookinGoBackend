using Booking.Contracts.Parameters;
using Booking.Contracts.Responses;
using Booking.DataAccess.Providers.Abstract;
using Booking.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;

namespace BookingApp.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/client/user/")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {

        private readonly UserAuthenticationService _authenticationService;
        private readonly IUserProvider _userProvider;

        public UserController(UserAuthenticationService authenticationService, IUserProvider userProvider)
        {
            _authenticationService = authenticationService;
            _userProvider = userProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
          
            var user = await _authenticationService.GetUserByHeaders(Request.Headers[HeaderNames.Authorization].ToArray());
            var response = new UserResponse()
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Rating = user.Rating,
                BirthDate = user.BirthDate
            };

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] UserParameter parameter)
        {
            var user = await _authenticationService.GetUserByHeaders(Request.Headers[HeaderNames.Authorization].ToArray());
            user.Phone = parameter.Phone;
            user.FullName = parameter.FullName;
            user.BirthDate = parameter.BirthDate;
            await _userProvider.Edit(user);

            return Ok();
        }

    }
    }
