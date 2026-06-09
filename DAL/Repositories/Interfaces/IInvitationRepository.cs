
using Domain.Projects;

namespace DAL.Repositories.Interfaces
{
    public interface IInvitationRepository
    {
        Task<Invitation?> GetAsync(
            int projectId,
            string userId);

        Task<List<Invitation>> GetUserInvitationsAsync(
            string userId);

        Task<List<Invitation>> GetProjectInvitationsAsync(int projectId);

        Task AddAsync(Invitation invitation);

        void Delete(Invitation invitation);

        void Update(Invitation invitation);
    }
}
