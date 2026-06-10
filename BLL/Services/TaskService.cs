
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using BLL.Services.Interface;


namespace BLL.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Tasks.Task> CreateTaskAsync(Domain.Tasks.Task task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<Domain.Tasks.Task?> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.Column)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Domain.Tasks.Task>> GetTasksByColumnIdAsync(int columnId)
        {
            return await _context.Tasks
                .Where(t => t.ColumnId == columnId)
                .ToListAsync();
        }

        public async Task<Domain.Tasks.Task> UpdateTaskAsync(Domain.Tasks.Task task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MoveTaskAsync(int taskId, int targetColumnId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return false;

            task.ColumnId = targetColumnId;
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
