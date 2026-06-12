using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskEntity = Domain.Tasks.Task;

namespace BLL.Services.Interface
{
    public interface ITaskService
    {
        Task<TaskEntity> CreateTaskAsync(string title, string? description, int boardId, int? columnId, string userId);
        Task<TaskEntity?> GetTaskByIdAsync(int taskId);
        Task<IEnumerable<TaskEntity>> GetTasksForColumnAsync(int columnId);
        Task<IEnumerable<TaskEntity>> GetTasksForBoardAsync(int boardId);
        Task UpdateTaskAsync(int taskId, string title, string description);
        Task MoveTaskAsync(int taskId, int newColumnId, string userId);
        Task DeleteTaskAsync(int taskId);
        Task LeaveTaskComment(int taskId, string userId, string text);
        Task<List<Domain.Tasks.TaskComment>> GetTaskCommentsAsync(int taskId);
        Task<Domain.Tasks.TaskAssignee> AssigneUser(int taskId, string userId);
        Task DeassigneUser(int taskId, string userId);
    }
}
