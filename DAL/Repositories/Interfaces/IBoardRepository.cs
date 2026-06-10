

using Domain.Boards;

namespace DAL.Repositories.Interfaces
{
    public interface IBoardRepository
    {
        Task<Board?> GetByIdAsync(int id);

        Task<List<Board>> GetProjectBoardsAsync(int projectId);

        Task AddAsync(Board board);

        void Update(Board board);

        void Delete(Board board);
    }
}
