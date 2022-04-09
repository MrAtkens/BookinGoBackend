using Booking.Contracts.Dtos;
using Booking.Contracts.Options;
using Booking.DataAccess.Providers.Abstract;
using Booking.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Booking.Services
{
    public class AdminAuthenticationService
    {
        private readonly IAdminProvider _adminProvider;

        private SecretOption SecretOptions { get; }

        public AdminAuthenticationService(IAdminProvider adminProvider, IOptions<SecretOption> secretOptions)
        {
            SecretOptions = secretOptions.Value;
            this._adminProvider = adminProvider;
        }


        /// <summary>
        ///     Get Jwt token by exited user
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <exception cref="ArgumentException">User is not found</exception>
        /// <returns>Jwt token</returns>
        public async Task<string> Authenticate(string login, string password)
        {
            // if user is not found, throw exception
            try
            {
                // Find data by arguments
                var admin = await _adminProvider.GetByLogin(login);

                if (!BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash))
                    throw new ArgumentException("Incorrect password");

                return GenerateJwtToken(admin.Login, admin.Role);
            }
            catch (ArgumentException e)
            {
                return null;
            }
        }

        private string GenerateJwtToken(string login, AdminRole role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretOptions.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.UserData, login),
                    new Claim(ClaimTypes.Role, role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }


        /// <summary>
        ///    Token decryption
        /// </summary>
        /// <param name="token"></param>
        /// <exception cref="ArgumentException">throws when could not parse claims</exception>
        /// <returns>Owner's data</returns>
        private AdminClaimsDto DecryptToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            if (tokenS?.Claims is List<Claim> claims)
            {
                return new AdminClaimsDto
                {
                    Login = claims[0].Value,
                    Role = (AdminRole)((Enum.TryParse(typeof(AdminRole), claims[1].Value, true, out var role)
                        ? role
                        : throw new ArgumentException()) ?? throw new ArgumentException())
                };
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// Gets Admin by headers from Request
        /// Usage in controllers: 
        /// GetAdminByHeaders(Request.Headers[HeaderNames.Authorization].ToArray())
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task<Admin> GetAdminByHeaders(string[] headers)
        {
            var token = headers[0].Replace("Bearer ", "");

            var result = await _adminProvider.GetByLogin(DecryptToken(token).Login);

            return result;
        }
    }

}
