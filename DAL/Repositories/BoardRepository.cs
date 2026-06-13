using DAL.Context;
using DAL.Repositories.Interfaces;
using Domain.Boards;
using Microsoft.EntityFrameworkCore;


namespace DAL.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private readonly ApplicationDbContext _context;

        public BoardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Board?> GetByIdAsync(int id)
        {
            return await _context.Boards
                .Include(b => b.Columns)
                .Include(b => b.Project)
                .Include(b => b.Tasks)
                .ThenInclude(t => t.Assignees)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Board>> GetProjectBoardsAsync(int projectId) 
        { 
            return await _context.Boards
                .Where(b => b.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task AddAsync(Board board) 
        { 
            await _context.Boards.AddAsync(board);
        }

        public void Update(Board board)
        {
            _context.Boards.Update(board);
        }

        public void Delete(Board board) 
        { 
            _context.Boards.Remove(board);
        }
    }
}
