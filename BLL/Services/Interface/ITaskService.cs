using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskEntity = Domain.Tasks.Task;

namespace BLL.Services.Interface
{
    public interface ITaskService
    {
        Task<TaskEntity> CreateTaskAsync(string title, string description, int columnId, string userId);
        Task<TaskEntity?> GetTaskByIdAsync(int taskId);
        Task<IEnumerable<TaskEntity>> GetTasksForColumnAsync(int columnId);
        Task<IEnumerable<TaskEntity>> GetTasksForBoardAsync(int boardId);
        Task UpdateTaskAsync(int taskId, string title, string description);
        Task MoveTaskAsync(int taskId, int newColumnId, string userId);
        Task DeleteTaskAsync(int taskId);
    }
}
