using System.Data.Common;
using Domain.Columns;

namespace TrelloClone.BLL.Services.Interface
{
    public interface IColumnService
    {
        Task<Domain.Columns.Column> CreateColumnAsync(string name, int boardId, Guid userId);
        Task<Domain.Columns.Column?> GetColumnByIdAsync(int columnId);
        Task<IEnumerable<Domain.Columns.Column>> GetColumnsForBoardAsync(int boardId);
        Task UpdateColumnAsync(int columnId, string name);
        Task DeleteColumnAsync(int columnId);
    }
}
