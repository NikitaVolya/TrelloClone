using Domain.Tasks;

namespace TrelloClone.BLL.Services.Interface
{
    public interface ITaskService
    {
        Task<Domain.Tasks.Task> CreateTaskAsync(string title, string description, int columnId, Guid userId);
        Task<Domain.Tasks.Task?> GetTaskByIdAsync(int taskId);
        Task<IEnumerable<Domain.Tasks.Task>> GetTasksForColumnAsync(int columnId);
        Task UpdateTaskAsync(int taskId, string title, string description);
        Task MoveTaskAsync(int taskId, int newColumnId, Guid userId);
        Task DeleteTaskAsync(int taskId);
    }
}
