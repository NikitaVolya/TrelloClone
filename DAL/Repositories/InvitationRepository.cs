using DAL.Context;
using DAL.Repositories.Interfaces;
using Domain.Projects;
using System.Data.Entity;


namespace DAL.Repositories
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly ApplicationDbContext _context;

        public InvitationRepository(ApplicationDbContext context) 
        { 
            _context = context;
        }

        public async Task<Invitation?> GetAsync(int projectId, string userId)
        {
            return await _context.Invitations
                .Include(inv => inv.User)
                .Include(inv => inv.Project)
                .Where(inv => inv.UserId == userId && inv.ProjectId == projectId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Invitation>> GetUserInvitationsAsync(string userId)
        {
            return await _context.Invitations
                .Where(inv => inv.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Invitation>> GetProjectInvitationsAsync(int projectId)
        {
            return await _context.Invitations
                .Where (inv => inv.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task AddAsync(Invitation invitation)
        {
            await _context.Invitations.AddAsync(invitation);
        }

        public void Delete(Invitation invitation)
        {
            _context.Invitations.Remove(invitation);
        }

        public void Update(Invitation invitation)
        {
            _context.Invitations.Update(invitation);
        }
    }
}
