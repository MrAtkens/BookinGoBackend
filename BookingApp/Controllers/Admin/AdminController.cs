using Booking.Contracts.Attributes;
using Booking.Contracts.Parameters;
using Booking.Contracts.Responses;
using Booking.DataAccess.Providers.Abstract;
using Booking.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Controllers.Admin
{
    [ApiExplorerSettings(GroupName = "v1-admin")]
    [Route("api/admin/")]
    [AdminAuthorized(roles: AdminRole.SuperAdmin)]
    public class AdminController : ControllerBase
    {
        private readonly IAdminProvider _adminProvider;

        public AdminController(IAdminProvider adminProvider)
        {
            _adminProvider = adminProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _adminProvider.GetAllResponses();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var admin = await _adminProvider.GetById(id);
            if (admin is null || admin.Role == AdminRole.SuperAdmin)
            {
                return NotFound();
            }
            var result = new GetAdminResponse
            {
                Id = admin.Id,
                Login = admin.Login,
                Role = admin.Role.ToString()
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AdminParameter parameter)
        {
            var moderator = await _adminProvider.FirstOrDefault(x => x.Login.Equals(parameter.Login));
            if (moderator is not null)
            {
                return BadRequest("Admin with current login existed");
            }
            var admin = new Booking.Models.Admin
            {
                Login = parameter.Login,
                Role = AdminRole.Moderator,
                PasswordHash = parameter.Password
            };
            await _adminProvider.Add(admin);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] AdminParameter parameter)
        {
            try
            {
                var moderator = await _adminProvider.FirstOrDefault(x => x.Login.Equals(parameter.Login));

                var admin = await _adminProvider.GetById(id) ?? throw new ArgumentException("No such admin");
                if (moderator is not null)
                {
                    if (!moderator.Id.Equals(admin.Id))
                        return BadRequest("Admin with current login existed");
                }
                if (admin is null || admin.Role == AdminRole.SuperAdmin)
                {
                    return NotFound();
                }

                admin.Login = parameter.Login;
                admin.PasswordHash = parameter.Password;
                await _adminProvider.Edit(admin);
                return NoContent();
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var admin = await _adminProvider.GetById(id);
            if (admin is null || admin.Role == AdminRole.SuperAdmin)
            {
                return NotFound();
            }
            await _adminProvider.Remove(admin);
            return NoContent();
        }


    }

}
