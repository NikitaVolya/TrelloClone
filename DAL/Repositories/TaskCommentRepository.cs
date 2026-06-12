using DAL.Context;
using DAL.Repositories.Interfaces;


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
            return _context.TaskComments.Where(tc => tc.TaskId == taskId).ToList();
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
