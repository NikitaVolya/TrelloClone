

namespace DAL.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<Domain.Tasks.Task?> GetByIdAsync(int id);

        Task<List<Domain.Tasks.Task>> GetBoardTasksAsync(int boardId);

        Task<List<Domain.Tasks.Task>> GetColumnTasksAsync(int columnId);

        Task AddAsync(Domain.Tasks.Task task);

        void Update(Domain.Tasks.Task task);

        void Delete(Domain.Tasks.Task task);
    }
}
