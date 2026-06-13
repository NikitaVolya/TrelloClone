using BLL.Services.Interface;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork.Interface;
using Domain.Common;
using Domain.Projects;
using TaskEntity = Domain.Tasks.Task;

namespace BLL.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IColumnRepository _columnRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskCommentRepository _taskCommentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TaskService(
            ITaskRepository taskRepository,
            IColumnRepository columnRepository,
            IBoardRepository boardRepository,
            IProjectRepository projectRepository,
            ITaskCommentRepository taskCommentRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _columnRepository = columnRepository;
            _boardRepository = boardRepository;
            _projectRepository = projectRepository;
            _taskCommentRepository = taskCommentRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TaskEntity> CreateTaskAsync(string name, string? description, int boardId, int? columnId, string userId)
        {

            var board = await _boardRepository.GetByIdAsync(boardId)
                ?? throw new ArgumentException("Board does not exist");

            var member = await _projectRepository.GetMemberAsync(board.ProjectId, userId);
            if (board.Project.OwnerId != userId && member == null)
                throw new UnauthorizedAccessException("User has no access to this project");

            var task = new TaskEntity
            {
                Name = name,
                Description = description,
                ColumnId = columnId,
                BoardId = boardId,
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

        public async Task MoveTaskAsync(int taskId, int? newColumnId, string userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId)
                ?? throw new ArgumentException("Task not found");

            if (newColumnId != null)
            {
                var newColumn = await _columnRepository.GetByIdAsync(newColumnId.Value)
                    ?? throw new ArgumentException("Target column does not exist");

                var board = await _boardRepository.GetByIdAsync(newColumn.BoardId)
                    ?? throw new ArgumentException("Board does not exist");


                var member = await _projectRepository.GetMemberAsync(board.ProjectId, userId);
                if (board.Project.OwnerId != userId && member == null)
                    throw new UnauthorizedAccessException("User has no access to this project");
            }

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

        public async Task LeaveTaskComment(int taskId, string userId, string text)
        {
            Domain.Tasks.Task? task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
                throw new InvalidOperationException($"Task with id {taskId} does not exist.");

            ApplicationUser? user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException($"User with id {userId} does not exist.");

            Domain.Tasks.TaskComment comment = new Domain.Tasks.TaskComment
            {
                TaskId = taskId,
                SenderId = userId,
                Text = text,
                CreatedAt = DateTime.UtcNow,
            };

            await _taskCommentRepository.AddTaskCommentAsync(comment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<Domain.Tasks.TaskComment>> GetTaskCommentsAsync(int taskId)
        {
            return await _taskCommentRepository.GetTaskCommentsAsync(taskId);
        }

        public async Task<Domain.Tasks.TaskAssignee> AssigneUser(int taskId, string userId)
        {
            if (await _taskRepository.GetTaskAssigneeAsync(taskId, userId) != null)
            {
                throw new InvalidOperationException($"User with id {taskId} is already assigneed to task with {taskId}");
            }

            Domain.Tasks.TaskAssignee assignee = new Domain.Tasks.TaskAssignee
            {
                TaskId = taskId,
                UserId = userId
            };

            await _taskRepository.AddTaskAssigneeAsync(assignee);
            await _unitOfWork.SaveChangesAsync();
            return assignee;
        }

        public async Task DeassigneUser(int taskId, string userId)
        {
            Domain.Tasks.TaskAssignee? assignee = await _taskRepository.GetTaskAssigneeAsync(taskId, userId);
            if (assignee == null)
            {
                throw new InvalidOperationException($"User with id {taskId} is not assigneed to task with {taskId}");
            }

            _taskRepository.RemoveTaskAssigneAsync(assignee);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task SetTaskDeadline(int taskId, DateTime? deadline)
        {
            Domain.Tasks.Task? task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
                throw new InvalidOperationException($"Task with id {taskId} does not exist.");

            task.Deadline = deadline;
            _taskRepository.Update(task);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
