
using DAL.Repositories.Interfaces;
using Domain.Common;
using Microsoft.AspNetCore.Identity;


namespace DAL.Repositories
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ApplicationUser?> RegisterAsync(
            string username,
            string email,
            string password)
        {
            ApplicationUser user = new()
            {
                UserName = username,
                Email = email
            };

            IdentityResult result =
                await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return null;

            return user;
        }

        public async Task<bool> LoginAsync(
            string email,
            string password)
        {
            ApplicationUser? user =
                await _userManager.FindByEmailAsync(email);

            if (user == null)
                return false;

            SignInResult result =
                await _signInManager.PasswordSignInAsync(
                    user,
                    password,
                    isPersistent: false,
                    lockoutOnFailure: false);

            return result.Succeeded;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
