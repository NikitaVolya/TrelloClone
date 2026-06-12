
using Domain.Common;


namespace BLL.Services.Interface
{
    public interface IAuthService
    {
        Task<ApplicationUser?> GetUserByIdAsync(string id);
        Task<bool> UserExistsAsync(string userId);
        Task<ApplicationUser?> RegisterAsync(string username, string email, string password);

        Task<bool> LoginAsync(string email, string password);

        Task LogoutAsync();
    }
}
