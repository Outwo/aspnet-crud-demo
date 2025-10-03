namespace CrudDemo.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; } // Untuk demo, plaintext. Nanti bisa hash.
    }
}
