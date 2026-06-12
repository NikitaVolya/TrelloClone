using DAL.Context;
using DAL.Repositories.Interfaces;
using Domain.Boards;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ColumnRepository : IColumnRepository
    {
        private readonly ApplicationDbContext _context;

        public ColumnRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Column?> GetByIdAsync(int id)
        {
            return await _context.Columns
                .Include(c => c.Board)
                .Include(c => c.Tasks)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Column>> GetBoardColumnsAsync(int boardId)
        {
            return await _context.Columns
                .Where(c => c.BoardId == boardId)
                .ToListAsync();
        }

        public async Task AddAsync(Column column)
        {
            await _context.Columns.AddAsync(column);
        }

        public void Update(Column column)
        {
            _context.Columns.Update(column);
        }

        public void Delete(Column column)
        {
            _context.Columns.Remove(column);
        }
    }
}
