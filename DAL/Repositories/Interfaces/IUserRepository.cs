
using Domain.Common;
using Microsoft.AspNetCore.Identity;


namespace DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetByIdAsync(string id);
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<ApplicationUser?> GetByUserNameAsync(string userName);

        Task<IdentityResult> CreateAsync(
            ApplicationUser user,
            string password);

        Task<bool> CheckPasswordAsync(
            ApplicationUser user,
            string password);
    }
}
