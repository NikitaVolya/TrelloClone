using BLL.Services.Interface;
using Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace MVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Profile()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser? user = await _userService.GetByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new { message = "Користувача не знайдено" });
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(string name, string email)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser? user = await _userService.GetByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new { message = "Користувача не знайдено" });
            }

            string userName = user.UserName;
            if (!string.IsNullOrWhiteSpace(name))
            {
                userName = name;
            }

            string userEmail = user.Email;
            if (!string.IsNullOrWhiteSpace(email))
            {
                userEmail = email;
            }

            await _userService.UpdateUserAsync(userId, userName, userEmail);

            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser? user = await _userService.GetByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new { message = "Користувача не знайдено" });
            }

            var result = await _userService.ChangePasswordAsync(userId, oldPassword, newPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                
                user = await _userService.GetByIdAsync(userId);
                return View("Profile", user);
            }

            TempData["Success"] = "Пароль успішно змінено";

            user = await _userService.GetByIdAsync(userId);
            return RedirectToAction("Profile", user);
        }
    }
}
