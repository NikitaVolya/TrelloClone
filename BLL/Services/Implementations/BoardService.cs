using System;
using BLL.Interfaces;
using DAL.Context;
using Domain.Boards;
using Microsoft.EntityFrameworkCore;
using TrelloClone.BLL.Servises.Interfaces;

namespace TrelloClone.BLL.Servises.Implementations
{
    public class BoardService : IBoardService
    {
        private readonly AppDbContext _context;

        public BoardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Board> CreateBoardAsync(Board board)
        {
            _context.Boards.Add(board);
            await _context.SaveChangesAsync();
            return board;
        }

        public async Task<Board?> GetBoardByIdAsync(int id)
        {
            return await _context.Boards
                .Include(b => b.Columns) 
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Board>> GetBoardsByProjectIdAsync(int projectId)
        {
            return await _context.Boards
                .Where(b => b.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<Board> UpdateBoardAsync(Board board)
        {
            _context.Boards.Update(board);
            await _context.SaveChangesAsync();
            return board;
        }

        public async Task<bool> DeleteBoardAsync(int id)
        {
            var board = await _context.Boards.FindAsync(id);
            if (board == null) return false;

            _context.Boards.Remove(board);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
