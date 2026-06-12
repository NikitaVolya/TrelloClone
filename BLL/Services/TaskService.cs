using BLL.Services.Interface;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork.Interface;
using TaskEntity = Domain.Tasks.Task;

namespace BLL.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IColumnRepository _columnRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TaskService(
            ITaskRepository taskRepository,
            IColumnRepository columnRepository,
            IBoardRepository boardRepository,
            IProjectRepository projectRepository,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _columnRepository = columnRepository;
            _boardRepository = boardRepository;
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TaskEntity> CreateTaskAsync(string name, string description, int columnId, string userId)
        {
            var column = await _columnRepository.GetByIdAsync(columnId)
                ?? throw new ArgumentException("Column does not exist");

            var board = await _boardRepository.GetByIdAsync(column.BoardId)
                ?? throw new ArgumentException("Board does not exist");

            var member = await _projectRepository.GetMemberAsync(board.ProjectId, userId);
            if (board.Project.OwnerId != userId && member == null)
                throw new UnauthorizedAccessException("User has no access to this project");

            var task = new TaskEntity
            {
                Name = name,
                Description = description,
                ColumnId = columnId,
                BoardId = column.BoardId,
                CratedAt = DateTime.UtcNow
            };

            await _taskRepository.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();
            return task;
        }

        public Task<TaskEntity?> GetTaskByIdAsync(int taskId) =>
            _taskRepository.GetByIdAsync(taskId);

        public async Task<IEnumerable<TaskEntity>> GetTasksForColumnAsync(int columnId) =>
            await _taskRepository.GetColumnTasksAsync(columnId);

        public async Task<IEnumerable<TaskEntity>> GetTasksForBoardAsync(int boardId) =>
            await _taskRepository.GetBoardTasksAsync(boardId);

        public async Task UpdateTaskAsync(int taskId, string name, string description)
        {
            var task = await _taskRepository.GetByIdAsync(taskId)
                ?? throw new ArgumentException("Task not found");

            task.Name = name;
            task.Description = description;
            _taskRepository.Update(task);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task MoveTaskAsync(int taskId, int newColumnId, string userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId)
                ?? throw new ArgumentException("Task not found");

            var newColumn = await _columnRepository.GetByIdAsync(newColumnId)
                ?? throw new ArgumentException("Target column does not exist");

            var board = await _boardRepository.GetByIdAsync(newColumn.BoardId)
                ?? throw new ArgumentException("Board does not exist");

            var member = await _projectRepository.GetMemberAsync(board.ProjectId, userId);
            if (board.Project.OwnerId != userId && member == null)
                throw new UnauthorizedAccessException("User has no access to this project");

            task.ColumnId = newColumnId;
            _taskRepository.Update(task);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int taskId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId)
                ?? throw new ArgumentException("Task not found");

            _taskRepository.Delete(task);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
