using Domain.Tasks;

namespace TrelloClone.Servises.Interfaces
{
    public interface ITaskService
    {
        Task<TaskItem> CreateTaskAsync(TaskItem task);
        Task<TaskItem?> GetTaskByIdAsync(int id);
        Task<IEnumerable<TaskItem>> GetTasksByColumnIdAsync(int columnId);
        Task<TaskItem> UpdateTaskAsync(TaskItem task);
        Task<bool> DeleteTaskAsync(int id);
        Task<bool> MoveTaskAsync(int taskId, int targetColumnId);
    }
}
