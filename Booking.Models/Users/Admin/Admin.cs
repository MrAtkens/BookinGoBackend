using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Models
{
    public class Admin : Entity
    {
        private string _passwordHash;
        public string Login { get; set; }

        public string PasswordHash
        {
            get => _passwordHash;
            set => _passwordHash = BCrypt.Net.BCrypt.HashPassword(value);
        }

        [Column(TypeName = "smallint")]
        public AdminRole Role { get; set; }

        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, this.PasswordHash);
        }

    }

}
