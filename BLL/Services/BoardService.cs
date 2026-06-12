using BLL.Services.Interface;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork.Interface;
using Domain.Boards;

namespace BLL.Services
{
    public class BoardService : IBoardService
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BoardService(IBoardRepository boardRepository, IUnitOfWork unitOfWork)
        {
            _boardRepository = boardRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Board> CreateBoardAsync(string title, int projectId)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Board title cannot be empty");

            var board = new Board
            {
                Title = title,
                ProjectId = projectId,
                CreatedAt = DateTime.UtcNow
            };

            await _boardRepository.AddAsync(board);
            await _unitOfWork.SaveChangesAsync();
            return board;
        }

        public Task<Board?> GetBoardByIdAsync(int boardId) =>
            _boardRepository.GetByIdAsync(boardId);

        public async Task<IEnumerable<Board>> GetBoardsForProjectAsync(int projectId) =>
            await _boardRepository.GetProjectBoardsAsync(projectId);

        public async Task UpdateBoardAsync(int boardId, string title)
        {
            var board = await _boardRepository.GetByIdAsync(boardId)
                ?? throw new ArgumentException("Board not found");

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Board title cannot be empty");

            board.Title = title;
            _boardRepository.Update(board);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteBoardAsync(int boardId)
        {
            var board = await _boardRepository.GetByIdAsync(boardId)
                ?? throw new ArgumentException("Board not found");

            _boardRepository.Delete(board);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
