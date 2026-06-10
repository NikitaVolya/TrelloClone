using Domain.Tasks;

namespace BLL.Services.Interface
{
    public interface ITaskService
    {
        Task<Domain.Tasks.Task> CreateTaskAsync(Domain.Tasks.Task task);
        Task<Domain.Tasks.Task?> GetTaskByIdAsync(int id);
        Task<IEnumerable<Domain.Tasks.Task>> GetTasksByColumnIdAsync(int columnId);
        Task<Domain.Tasks.Task> UpdateTaskAsync(Domain.Tasks.Task task);
        Task<bool> DeleteTaskAsync(int id);
        Task<bool> MoveTaskAsync(int taskId, int targetColumnId);
    }
}
