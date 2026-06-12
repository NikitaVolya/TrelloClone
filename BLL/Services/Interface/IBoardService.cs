namespace BLL.Services.Interface
{
    public interface IBoardService
    {
        Task<Domain.Boards.Board> CreateBoardAsync(string title, int projectId);
        Task<Domain.Boards.Board?> GetBoardByIdAsync(int boardId);
        Task<IEnumerable<Domain.Boards.Board>> GetBoardsForProjectAsync(int projectId);
        Task UpdateBoardAsync(int boardId, string title);
        Task DeleteBoardAsync(int boardId);
        Task<List<Domain.Tasks.TaskComment>> GetBoardTaskComments(int boardId);
    }
}
