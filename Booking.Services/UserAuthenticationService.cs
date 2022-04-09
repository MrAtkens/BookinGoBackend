using System.Security.Claims;
using System.Text;
using Booking.Contracts.Dtos;
using Booking.Contracts.Options;
using Booking.DataAccess.Providers.Abstract;
using Booking.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;



namespace Booking.Services
{
    public class UserAuthenticationService
    {
        private readonly IUserProvider _provider;

        private SecretOption SecretOptions { get; }

        public UserAuthenticationService(IUserProvider provider, IOptions<SecretOption> secretOptions)
        {
            SecretOptions = secretOptions.Value;
            this._provider = provider;
        }

        /// <summary>
        ///     Getting a token before creating a new user
        /// </summary>
        /// <param name="newUser">Data transfer object for registration new user</param>
        /// <exception cref="ArgumentException">User is already exists</exception>
        /// <returns></returns>
        public async Task<string> Register(UserRegistrationDto newUser)
        {
            // Try to get a data from the newUser parameter
            var user = await _provider.FirstOrDefault(x => x.Email == newUser.Email);

            if (user is not null)
                throw new ArgumentException("This user is already exists");


            // Add new user to table
            await _provider.Add(new UserRegistrationDto
            {
                FullName = newUser.FullName,
                Email = newUser.Email,
                Phone = newUser.Phone,
                BirthDate = newUser.BirthDate,
                Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password),
            });

            return GenerateJwtToken(newUser.Email);
        }



        /// <summary>
        ///     Get Jwt token by exited user
        /// </summary>
        /// <param name="emailOrPhone"></param>
        /// <param name="password"></param>
        /// <exception cref="ArgumentException">User is not found</exception>
        /// <returns>Jwt token</returns>
        public async Task<string> Authenticate(string emailOrPhone, string password)
        {
            try
            {
                // Find data by arguments
                var user = await _provider.GetByEmailOrPhone(emailOrPhone);

                // if user is not found, throw exception
                if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                    throw new ArgumentException("Incorrect password");

                return GenerateJwtToken(user.Email);
            }
            catch (ArgumentException e)
            {
                return null;
            }
        }

        public async Task<string> AuthenticateConfirmUser(Guid userId)
        {
            var user = await _provider.GetById(userId) ??
                throw new ArgumentException("This user doesn't exist.");

            user.IsConfirmed = true;
            await _provider.Edit(user);
            return GenerateJwtToken(user.Email);
        }


        private string GenerateJwtToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = SecretOptions.JwtSecret;
            var key = Encoding.ASCII.GetBytes(SecretOptions.JwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
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
        private UserClaimsDto DecryptToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var tokenS = handler.ReadToken(token) as JwtSecurityToken;
        
            if (tokenS?.Claims is List<Claim> claims)
            {
                return new UserClaimsDto()
                {
                    Email = claims[0].Value
                };
            }

            throw new ArgumentException();
        }


        /// <summary>
        /// Gets User by headers from Request
        /// Usage in controllers: 
        /// GetUserByHeaders(Request.Headers[HeaderNames.Authorization].ToArray())
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task<User> GetUserByHeaders(string[] headers)
        {
            var token = headers[0].Replace("Bearer ", "");

            return await _provider.GetByEmailOrPhone(DecryptToken(token).Email);
        }
    }

}
