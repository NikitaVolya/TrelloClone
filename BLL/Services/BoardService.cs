using DAL.UnitOfWork.Interface;
using Domain.Boards;
using TrelloClone.BLL.Services.Interface;

public class BoardService : IBoardService
{
    private readonly IUnitOfWork _unitOfWork;

    public BoardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Board> CreateBoardAsync(string name, Guid ownerId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Board name cannot be empty");

        var board = new Board { Name = name, OwnerId = ownerId };
        await _unitOfWork.Boards.AddAsync(board);
        await _unitOfWork.SaveChangesAsync();
        return board;
    }

    public Task<Board?> GetBoardByIdAsync(int boardId) =>
        _unitOfWork.Boards.GetByIdAsync(boardId);

    public Task<IEnumerable<Board>> GetBoardsForUserAsync(Guid userId) =>
        _unitOfWork.Boards.GetByOwnerIdAsync(userId);

    public async Task UpdateBoardAsync(int boardId, string name)
    {
        var board = await _unitOfWork.Boards.GetByIdAsync(boardId)
            ?? throw new ArgumentException("Board not found");

        board.Name = name;
        _unitOfWork.Boards.Update(board);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteBoardAsync(int boardId)
    {
        var board = await _unitOfWork.Boards.GetByIdAsync(boardId)
            ?? throw new ArgumentException("Board not found");

        _unitOfWork.Boards.Delete(board);
        await _unitOfWork.SaveChangesAsync();
    }
}
