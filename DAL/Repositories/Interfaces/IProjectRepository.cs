
using Domain.Projects;

namespace DAL.Repositories.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(int id);
        Task<List<Project>> GetUserProjectsAsync(string userId);
        Task<List<Project>> GetOwnerProjectsAsync(string userId);
        Task AddAsync(Project project);
        void Update(Project project);
        void Delete(Project project);
        Task<ProjectMember?> GetMemberAsync(int projectId, string memberId);
        Task AddMemberAsync(ProjectMember projectMember);
        void RemoveMemberAsync(ProjectMember projectMember);
    }
}
