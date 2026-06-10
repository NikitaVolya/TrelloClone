using Domain.Boards;

namespace BLL.Services.Interface
{
    public interface IBoardService
    {
        Task<Board> CreateBoardAsync(Board board);
        Task<Board?> GetBoardByIdAsync(int id);
        Task<IEnumerable<Board>> GetBoardsByProjectIdAsync(int projectId);
        Task<Board> UpdateBoardAsync(Board board);
        Task<bool> DeleteBoardAsync(int id);
    }
}
