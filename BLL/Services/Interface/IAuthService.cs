using Domain.Common;

namespace DAL.Repositories.Interfaces
{
    public interface IAuthService
    {
        Task<ApplicationUser?> RegisterAsync(
        string username,
        string email,
        string password);

        Task<bool> LoginAsync(
            string email,
            string password);

        Task LogoutAsync();
    }
}
