using Domain.Boards;

namespace DAL.Repositories.Interfaces
{
    public interface IColumnRepository
    {
        Task<Column?> GetByIdAsync(int id);
        Task<List<Column>> GetBoardColumnsAsync(int boardId);
        Task AddAsync(Column column);
        void Update(Column column);
        public void Update(IEnumerable<Column> columns);
        void Delete(Column column);
    }
}
