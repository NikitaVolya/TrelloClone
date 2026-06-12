using Domain.Projects;


namespace BLL.Services.Interface
{
    public interface IProjectService
    {
        Task<Project?> CreateAsync(
            string title,
            string? description,
            string ownerId);

        Task<Project?> GetByIdAsync(int id);

        Task<List<Project>> GetUserProjectsAsync(string userId);
        Task<List<Project>> GetOwnerProjectsAsync(string ownerId);

        Task<bool> DeleteAsync(int projectId, string userId);

        Task<bool> UpdateAsync(
            int projectId,
            string title,
            string? description);

        Task AddMemberAsync(int projectId, string memberId);

        Task RemoveMemberAsync(int projectId, string memberId);

        Task<bool> CheckUserIsProjectMember(int projectId, string userId);
    }
}
