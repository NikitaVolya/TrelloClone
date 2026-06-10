using DAL.Context;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace DAL.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Tasks.Task?> GetByIdAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.Column)
                .Include(t => t.Assignees)
                    .ThenInclude(a => a.User)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.Sender)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Domain.Tasks.Task>> GetBoardTasksAsync(int boardId)
        {
            return await _context.Tasks
                .Where(t => t.BoardId == boardId)
                .ToListAsync();
        }

        public async Task<List<Domain.Tasks.Task>> GetColumnTasksAsync(int columnId)
        {
            return await _context.Tasks
                .Where(t => t.ColumnId == columnId)
                .ToListAsync();
        }

        public async Task MoveToColumnAsync(int taskId, int? columnId)
        {
            var task = await _context.Tasks.FindAsync(taskId);

            if (task == null)
                return;

            task.ColumnId = columnId;
        }

        public async Task AddAsync(Domain.Tasks.Task task)
        {
            await _context.Tasks.AddAsync(task);
        }

        public void Update(Domain.Tasks.Task task)
        {
            _context.Tasks.Update(task);
        }

        public void Delete(Domain.Tasks.Task task)
        {
            _context.Tasks.Remove(task);
        }
    }
}
