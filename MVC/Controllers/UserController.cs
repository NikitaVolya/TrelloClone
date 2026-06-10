using Microsoft.AspNetCore.Mvc;
using Domain.Users;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        public static readonly List<User> MockUsers = new List<User>
        {
            new User
            {
                Id = "user1",
                Name = "Іван Петренко",
                Email = "ivan@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                CreatedAt = DateTime.Now.AddMonths(-6)
            }
        };

        public IActionResult Profile()
        {
            var currentUser = MockUsers.FirstOrDefault(u => u.Id == "user1");
            if (currentUser == null)
            {
                return NotFound(new { message = "Користувача не знайдено" });
            }
            return View(currentUser);
        }

        [HttpPost]
        public IActionResult UpdateProfile(string name, string email)
        {
            var currentUser = MockUsers.FirstOrDefault(u => u.Id == "user1");
            if (currentUser == null)
            {
                return NotFound(new { message = "Користувача не знайдено" });
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                currentUser.Name = name;
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                currentUser.Email = email;
            }

            return RedirectToAction("Profile");
        }

        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword)
        {
            var currentUser = MockUsers.FirstOrDefault(u => u.Id == "user1");
            if (currentUser == null)
            {
                return NotFound(new { message = "Користувача не знайдено" });
            }

            if (!BCrypt.Net.BCrypt.Verify(oldPassword, currentUser.PasswordHash))
            {
                TempData["Error"] = "Старий пароль невірний";
                return RedirectToAction("Profile");
            }

            currentUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            TempData["Success"] = "Пароль успішно змінено";

            return RedirectToAction("Profile");
        }
    }
}
