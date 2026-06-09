
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

        public async Task<List<Project>> GetUserProjectsAsync(string userId)
        {
            return await _context.Projects
                .Where(p => p.OwnerId == userId)
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
    }
}
