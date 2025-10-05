using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace CrudDemo.Models
{
    public class User
    {
        public int Id { get; set; } // Id unik

        [Required(ErrorMessage = "Username harus diisi")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password harus diisi")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Hash password dengan SHA256
        public void HashPassword()
        {
            if (string.IsNullOrWhiteSpace(this.Password)) return;

            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(this.Password);
            var hash = sha256.ComputeHash(bytes);
            this.Password = Convert.ToBase64String(hash);
        }
    }
}
