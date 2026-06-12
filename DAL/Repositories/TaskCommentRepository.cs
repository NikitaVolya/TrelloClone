using DAL.Context;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace DAL.Repositories
{
    public class TaskCommentRepository : ITaskCommentRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskCommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Domain.Tasks.TaskComment>> GetTaskCommentsAsync(int taskId)
        {
            return await _context.TaskComments.Where(tc => tc.TaskId == taskId).ToListAsync();
        }
        public async Task<List<Domain.Tasks.TaskComment>> GetTaskCommentsByBoardIdAsync(int boardId)
        {
            return await _context.TaskComments
                .Include(c => c.Sender)
                .Include(c => c.Task)
                .Where(c => c.Task.BoardId == boardId).ToListAsync();
        }

        public async Task AddTaskCommentAsync(Domain.Tasks.TaskComment comment)
        {
            await _context.TaskComments.AddAsync(comment);
        }

        public void DeleteTaskComment(Domain.Tasks.TaskComment comment)
        {
            _context.TaskComments.Remove(comment);
        }
    }
}
