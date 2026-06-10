
using DAL.Context;
using Domain.Boards;
using Microsoft.EntityFrameworkCore;
using TrelloClone.BLL.Services.Interface;


namespace TrelloClone.Servises.Implementations
{
    namespace BLL.Implementations
    {
        public class ColumnService : IColumnService
        {
            private readonly ApplicationDbContext _context;

            public ColumnService(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Column> CreateColumnAsync(Column column)
            {
                _context.Columns.Add(column);
                await _context.SaveChangesAsync();
                return column;
            }

            public async Task<Column?> GetColumnByIdAsync(int id)
            {
                return await _context.Columns
                    .Include(c => c.Tasks)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }

            public async Task<IEnumerable<Column>> GetColumnsByBoardIdAsync(int boardId)
            {
                return await _context.Columns
                    .Where(c => c.BoardId == boardId)
                    .ToListAsync();
            }

            public async Task<Column> UpdateColumnAsync(Column column)
            {
                _context.Columns.Update(column);
                await _context.SaveChangesAsync();
                return column;
            }

            public async Task<bool> DeleteColumnAsync(int id)
            {
                var column = await _context.Columns.FindAsync(id);
                if (column == null) return false;

                _context.Columns.Remove(column);
                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
}
