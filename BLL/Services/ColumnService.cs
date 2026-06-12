using BLL.Services.Interface;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork.Interface;
using Domain.Boards;
using Domain.Common;

namespace BLL.Services
{
    public class ColumnService : IColumnService
    {
        private readonly IColumnRepository _columnRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public ColumnService(
            IColumnRepository columnRepository,
            IBoardRepository boardRepository,
            IProjectRepository projectRepository,
            IAuthService authService,
            IUnitOfWork unitOfWork)
        {
            _columnRepository = columnRepository;
            _boardRepository = boardRepository;
            _projectRepository = projectRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Column> CreateColumnAsync(string name, int boardId, string userId)
        {
            var board = await _boardRepository.GetByIdAsync(boardId)
                ?? throw new ArgumentException("Board does not exist");

            ApplicationUser? user = await _authService.GetUserByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User does not exists.");

            var member = await _projectRepository.GetMemberAsync(board.ProjectId, userId);
            if (board.Project.OwnerId != userId && member == null)
                throw new UnauthorizedAccessException("User has no access to this board");

            var column = new Column
            {
                Name = name,
                BoardId = boardId
            };

            await _columnRepository.AddAsync(column);
            await _unitOfWork.SaveChangesAsync();
            return column;
        }

        public Task<Column?> GetColumnByIdAsync(int columnId) =>
            _columnRepository.GetByIdAsync(columnId);

        public async Task<IEnumerable<Column>> GetColumnsForBoardAsync(int boardId) =>
            await _columnRepository.GetBoardColumnsAsync(boardId);

        public async Task UpdateColumnAsync(int columnId, string name)
        {
            var column = await _columnRepository.GetByIdAsync(columnId)
                ?? throw new ArgumentException("Column not found");

            column.Name = name;
            _columnRepository.Update(column);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteColumnAsync(int columnId)
        {
            var column = await _columnRepository.GetByIdAsync(columnId)
                ?? throw new ArgumentException("Column not found");

            _columnRepository.Delete(column);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
