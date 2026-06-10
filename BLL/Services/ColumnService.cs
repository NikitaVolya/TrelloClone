using DAL.UnitOfWork.Interface;
using Domain.Boards;
using TrelloClone.BLL.Services.Interface;

public class ColumnService : IColumnService
{
    private readonly IUnitOfWork _unitOfWork;

    public ColumnService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Column> CreateColumnAsync(string name, int boardId, Guid userId)
    {
        var board = await _unitOfWork.Boards.GetByIdAsync(boardId)
            ?? throw new ArgumentException("Board does not exist");

        var hasAccess = await _unitOfWork.Projects.HasAccessAsync(board.ProjectId, userId);
        if (!hasAccess)
            throw new UnauthorizedAccessException("User has no access to this board");

        var column = new Column { Name = name, BoardId = boardId };
        await _unitOfWork.Columns.AddAsync(column);
        await _unitOfWork.SaveChangesAsync();
        return column;
    }

    public Task<Column?> GetColumnByIdAsync(int columnId) =>
        _unitOfWork.Columns.GetByIdAsync(columnId);

    public Task<IEnumerable<Column>> GetColumnsForBoardAsync(int boardId) =>
        _unitOfWork.Columns.GetByBoardIdAsync(boardId);

    public async Task UpdateColumnAsync(int columnId, string name)
    {
        var column = await _unitOfWork.Columns.GetByIdAsync(columnId)
            ?? throw new ArgumentException("Column not found");

        column.Name = name;
        _unitOfWork.Columns.Update(column);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteColumnAsync(int columnId)
    {
        var column = await _unitOfWork.Columns.GetByIdAsync(columnId)
            ?? throw new ArgumentException("Column not found");

        _unitOfWork.Columns.Delete(column);
        await _unitOfWork.SaveChangesAsync();
    }
}
