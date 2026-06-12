using Domain.Boards;

namespace BLL.Services.Interface
{
    public interface IColumnService
    {
        Task<Column> CreateColumnAsync(string name, int boardId, string userId);
        Task<Column?> GetColumnByIdAsync(int columnId);
        Task<IEnumerable<Column>> GetColumnsForBoardAsync(int boardId);
        Task UpdateColumnAsync(int columnId, string name);
        Task DeleteColumnAsync(int columnId);
    }
}
