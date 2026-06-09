
using Domain.Projects;

namespace DAL.Repositories.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(int id);
        Task<List<Project>> GetUserProjectsAsync(string userId);

        Task AddAsync(Project project);
        void Update(Project project);
        void Delete(Project project);
    }
}
