using BLL.Services.Interface;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork.Interface;
using Domain.Projects;



namespace BLL.Services
{
    public class ProjectService : IProjectService
    {

        private readonly IProjectRepository _projectRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public int MaxOwnerProjectsCount => 3;

        public ProjectService(IProjectRepository projectRepository, IUserService userService, IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Project?> CreateAsync(
            string title,
            string? description,
            string ownerId)
        {

            if (!(await _userService.UserExistsAsync(ownerId)))
                throw new InvalidOperationException($"User with id '{ownerId}' does not exist.");

            if (string.IsNullOrEmpty(title))
                throw new ArgumentException("Title is required");

            Project newProject = new Project
            {
                Title = title,
                Description = description,
                OwnerId = ownerId,
                CreatedAt = DateTime.UtcNow,
            };

            await _projectRepository.AddAsync(newProject);
            await _unitOfWork.SaveChangesAsync();

            return await _projectRepository.GetByIdAsync(newProject.Id);
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _projectRepository.GetByIdAsync(id);
        }

        public async Task<List<Project>> GetUserProjectsAsync(string userId)
        {
            if (!(await _userService.UserExistsAsync(userId)))
                throw new InvalidOperationException($"User with id '{userId}' does not exist.");

            return await _projectRepository.GetUserProjectsAsync(userId);
        }

        public async Task<List<Project>> GetOwnerProjectsAsync(string ownerId)
        {
            if (!(await _userService.UserExistsAsync(ownerId)))
                throw new InvalidOperationException($"User with id '{ownerId}' does not exist.");

            return await _projectRepository.GetOwnerProjectsAsync(ownerId);
        }

        public async Task<bool> DeleteAsync(int projectId, string userId)
        {
            Project? project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                return false;

            if (!(await _userService.UserExistsAsync(userId)))
                throw new InvalidOperationException($"User with id '{userId}' does not exist.");

            if (project.OwnerId != userId)
                return false;

            _projectRepository.Delete(project);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(
            int projectId,
            string title,
            string? description)
        {
            Project? project = await GetByIdAsync(projectId);

            if (project == null)
                return false;

            project.Title = title;
            project.Description = description;

            _projectRepository.Update(project);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task AddMemberAsync(int projectId, string memberId)
        {
            Project? project = await _projectRepository.GetByIdAsync(projectId);

            if (project == null)
                throw new InvalidOperationException($"Project with id '{projectId}' does not exist.");

            if (!(await _userService.UserExistsAsync(memberId)))
                throw new InvalidOperationException($"User with id '{memberId}' does not exist.");

            if (project.OwnerId == memberId)
                throw new InvalidOperationException($"User is the owner of the project and cannot be added as a member.");


            ProjectMember? projectMember = await _projectRepository.GetMemberAsync(projectId, memberId);
            if (projectMember != null)
                throw new InvalidOperationException("User is already a member of this project.");

            projectMember = new ProjectMember
            {
                ProjectId = projectId,
                MemberId = memberId,
                JoinedAt = DateTime.UtcNow,
            };

            await _projectRepository.AddMemberAsync(projectMember);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(int projectId, string memberId)
        {
            Project? project = await _projectRepository.GetByIdAsync(projectId);

            if (project == null)
                throw new InvalidOperationException($"Project with id '{projectId}' does not exist.");

            if (!(await _userService.UserExistsAsync(memberId)))
                throw new InvalidOperationException($"User with id '{memberId}' does not exist.");

            ProjectMember? projectMember = await _projectRepository.GetMemberAsync(projectId, memberId);
            if (projectMember == null)
                throw new InvalidOperationException("User is not a member of this project.");

            _projectRepository.RemoveMemberAsync(projectMember);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> CheckUserIsProjectMember(int projectId, string userId)
        {
            ProjectMember? member = await _projectRepository.GetMemberAsync(projectId, userId);
            return member != null;
        }

    }
}
