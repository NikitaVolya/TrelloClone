using System.Data.Common;
using Domain.Columns;

namespace TrelloClone.Servises.Interfaces
{
    public interface IColumnService
    {
        Task<Column> CreateColumnAsync(Column column);
        Task<Column?> GetColumnByIdAsync(int id);
        Task<IEnumerable<Column>> GetColumnsByBoardIdAsync(int boardId);
        Task<Column> UpdateColumnAsync(Column column);
        Task<bool> DeleteColumnAsync(int id);
    }
}
