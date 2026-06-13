

namespace DAL.Repositories.Interfaces
{
    public interface ITaskCommentRepository
    {
        Task<List<Domain.Tasks.TaskComment>> GetTaskCommentsAsync(int taskId);
        Task<List<Domain.Tasks.TaskComment>> GetTaskCommentsByBoardIdAsync(int boardId);
        Task AddTaskCommentAsync(Domain.Tasks.TaskComment comment);
        void DeleteTaskComment(Domain.Tasks.TaskComment taskId);

    }
}
