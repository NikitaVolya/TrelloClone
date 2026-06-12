
using Microsoft.AspNetCore.Mvc;
using MVC.Models.Account;
using MVC.Services;

namespace MVC.Controllers
{
    public class AccountController : Controller
    {

        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _authService.RegisterAsync(
                model.UserName,
                model.Email,
                model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Помилка реєстрації");
                return View(model);
            }

            await _authService.LoginAsync(model.Email, model.Password);

            TempData["Success"] = "Реєстрація успішна";

            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await _authService.LogoutAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool success = await _authService.LoginAsync(
                model.Email,
                model.Password);

            if (!success)
            {
                ModelState.AddModelError("", "Невірний email або пароль");
                return View(model);
            }

            TempData["Success"] = "Вхід успішний";

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();

            return RedirectToAction("Login");
        }
    }
}
