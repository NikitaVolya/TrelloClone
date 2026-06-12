

using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services.Interface
{
    public interface IUserService
    {
        Task<ApplicationUser?> GetByIdAsync(string userId);
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<bool> UserExistsAsync(string userId);

        Task<IdentityResult> UpdateUserAsync(string userId, string newUserName, string newEmail);

        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);

        Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword);
    }
}
