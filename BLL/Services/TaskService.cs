using System;
using BLL.Interfaces;
using DAL.Context;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using TrelloClone.BLL.Services.Interface;

namespace TrelloClone.BLL.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.Column)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByColumnIdAsync(int columnId)
        {
            return await _context.Tasks
                .Where(t => t.ColumnId == columnId)
                .ToListAsync();
        }

        public async Task<TaskItem> UpdateTaskAsync(TaskItem task)
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
