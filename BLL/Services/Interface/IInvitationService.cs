

using Domain.Projects;

namespace BLL.Services.Interface
{
    public interface IInvitationService
    {
        Task InviteUserAsync(int projectId, string ownerId, string userId);

        Task<List<Invitation>> GetUserInvitationsAsync(string userId);

        Task AcceptInvitationAsync(int projectId, string userId);

        Task DeclineInvitationAsync(int projectId, string userId);

        Task<Invitation?> GetInvitationAsync(int projectId, string userId);
    }
}
