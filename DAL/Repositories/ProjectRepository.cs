
using Microsoft.EntityFrameworkCore;
using Domain.Projects;
using DAL.Context;
using DAL.Repositories.Interfaces;


namespace DAL.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Boards)
                .Include(p => p.Members)
                .Include(p => p.Invitations)
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Project>> GetOwnerProjectsAsync(string userId)
        {
            return await _context.Projects
                .Where(p => p.OwnerId == userId)
                .ToListAsync();
        }
        public async Task<List<Project>> GetUserProjectsAsync(string userId)
        {
            return await _context.Projects
                .Include(p => p.Members)
                .Where(p => p.OwnerId == userId || p.Members.Any(pm => pm.MemberId == userId))
                .ToListAsync();
        }

        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
        }

        public void Update(Project project)
        {
            _context.Projects.Update(project);
        }

        public void Delete(Project project)
        {
            _context.Projects.Remove(project);
        }
        public async Task<ProjectMember?> GetMemberAsync(int projectId, string memberId)
        {
            return await _context.ProjectMembers.FirstOrDefaultAsync(pm => pm.ProjectId == projectId && pm.MemberId == memberId);
        }

        public async Task AddMemberAsync(ProjectMember projectMember)
        {
            await _context.ProjectMembers.AddAsync(projectMember);
        }

        public void RemoveMemberAsync(ProjectMember projectMember)
        {
            _context.ProjectMembers.Remove(projectMember);
        }
    }
}
