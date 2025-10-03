using CrudDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrudDemo.Controllers
{
    public class AccountController : Controller
    {
        // User demo (In-Memory/Local)
        private readonly List<User> _users = new()
        {
            new User { Username = "admin", Password = "1234" },
            new User { Username = "user", Password = "abcd" }
        };

        // GET: /Account/Login
        public IActionResult Login()
        {
            // Ambil notifikasi logout atau timeout
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            var user = _users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                // Simpan session sederhana
                HttpContext.Session.SetString("username", user.Username);
                // Tambahkan notifikasi login berhasil
                TempData["LoginMessage"] = $"Selamat datang, {user.Username}!";
                return RedirectToAction("Index", "Students"); // Redirect ke CRUD
            }

            ViewBag.Error = "Username atau password salah!";
            return View();
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Message"] = "Anda telah logout.";
            return RedirectToAction("Login");
        }
    }
}
