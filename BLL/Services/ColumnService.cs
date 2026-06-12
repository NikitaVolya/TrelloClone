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
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public ColumnService(
            IColumnRepository columnRepository,
            IBoardRepository boardRepository,
            IProjectRepository projectRepository,
            IUserService userService,
            IUnitOfWork unitOfWork)
        {
            _columnRepository = columnRepository;
            _boardRepository = boardRepository;
            _projectRepository = projectRepository;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Column> CreateColumnAsync(string name, int boardId, string userId)
        {
            var board = await _boardRepository.GetByIdAsync(boardId)
                ?? throw new ArgumentException("Board does not exist");

            ApplicationUser? user = await _userService.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User does not exists.");

            var member = await _projectRepository.GetMemberAsync(board.ProjectId, userId);
            if (board.Project.OwnerId != userId && member == null)
                throw new UnauthorizedAccessException("User has no access to this board");

            List<Column> columns = await _columnRepository.GetBoardColumnsAsync(boardId);

            var column = new Column
            {
                Name = name,
                BoardId = boardId,
                Order = columns.Count()
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
        public async Task ChangeOrder(int columnId, int newOrder)
        {
            Column? column = await GetColumnByIdAsync(columnId);
            if (column == null)
                throw new InvalidOperationException("Column does not exist.");

            IEnumerable<Column> boardColumns = await GetColumnsForBoardAsync(column.BoardId);

            int oldOrder = column.Order;

            boardColumns = boardColumns.Where(c => c.Order >= newOrder && c.Order < oldOrder);

            foreach (Column boardColumn in boardColumns)
            {
                boardColumn.Order += (newOrder < oldOrder ? 1 : -1);
            }

            column.Order = newOrder;

            _columnRepository.Update(boardColumns);
            _columnRepository.Update(column);
            await _unitOfWork.SaveChangesAsync();

        }
    }
}
