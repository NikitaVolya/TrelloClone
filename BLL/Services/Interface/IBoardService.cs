namespace TrelloClone.BLL.Services.Interface
{
    public interface IBoardService
    {
        public interface IBoardService
        {
            Task<Domain.Boards.Board> CreateBoardAsync(string name, Guid ownerId);
            Task<Domain.Boards.Board?> GetBoardByIdAsync(int boardId);
            Task<IEnumerable<Domain.Boards.Board>> GetBoardsForUserAsync(Guid userId);
            Task UpdateBoardAsync(int boardId, string name);
            Task DeleteBoardAsync(int boardId);
        }
    }
