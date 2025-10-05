using CrudDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace CrudDemo.Controllers
{
    public class AccountController : Controller
    {
        // Demo: in-memory user storage
        private static List<User> _users = new()
        {
            new User { Username = "admin", Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("1234")) },
            new User { Username = "user", Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("abcd")) }
        };
        private static int _nextId = 3;

        // GET: /Account/Login
        public IActionResult Login()
        {
            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"];
            if (TempData["LoginMessage"] != null)
                ViewBag.LoginMessage = TempData["LoginMessage"];
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            // Hash password input agar sama dengan yang tersimpan
            var hashInput = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));

            var user = _users.FirstOrDefault(u => u.Username == username && u.Password == hashInput);
            if (user != null)
            {
                HttpContext.Session.SetString("username", user.Username);
                TempData["LoginMessage"] = $"Selamat datang, {user.Username}!";
                return RedirectToAction("Index", "Students"); // Redirect ke CRUD
            }

            ViewBag.Error = "Username atau password salah!";
            return View();
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User model)
        {
            // 1. Validasi model
            if (!ModelState.IsValid)
                return View(model);

            // 2. Cek username unik
            if (_users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("", "Username sudah digunakan!");
                return View(model);
            }

            // 3. Hash password dan simpan
            model.HashPassword();
            _users.Add(model);

            // 4. Redirect ke login dengan pesan sukses
            TempData["Message"] = "Registrasi berhasil! Silahkan login.";
            return RedirectToAction("Login");
        }


        // GET: /Account/Logout
        // GET: /Account/Logout
       // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["LogoutMessage"] = "Anda telah logout.";
            return RedirectToAction("Login");
        }


    }
}
