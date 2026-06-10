using DAL.UnitOfWork.Interface;
using TrelloClone.BLL.Services.Interface;

public class TaskService : ITaskService
{
    private readonly IUnitOfWork _unitOfWork;

    public TaskService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Task> CreateTaskAsync(string title, string description, int columnId, Guid userId)
    {
        var column = await _unitOfWork.Columns.GetByIdAsync(columnId)
            ?? throw new ArgumentException("Column does not exist");

        var hasAccess = await _unitOfWork.Projects.HasAccessAsync(column.ProjectId, userId);
        if (!hasAccess)
            throw new UnauthorizedAccessException("User has no access to this project");

        var task = new Task
        {
            Title = title,
            Description = description,
            ColumnId = columnId,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };

        await _unitOfWork.Tasks.AddAsync(task);
        await _unitOfWork.SaveChangesAsync();
        return task;
    }

    public Task<Task?> GetTaskByIdAsync(int taskId) =>
        _unitOfWork.Tasks.GetByIdAsync(taskId);

    public Task<IEnumerable<Task>> GetTasksForColumnAsync(int columnId) =>
        _unitOfWork.Tasks.GetByColumnIdAsync(columnId);

    public async Task UpdateTaskAsync(int taskId, string title, string description)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(taskId)
            ?? throw new ArgumentException("Task not found");

        task.Title = title;
        task.Description = description;
        _unitOfWork.Tasks.Update(task);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task MoveTaskAsync(int taskId, int newColumnId, Guid userId)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(taskId)
            ?? throw new ArgumentException("Task not found");

        var newColumn = await _unitOfWork.Columns.GetByIdAsync(newColumnId)
            ?? throw new ArgumentException("Target column does not exist");

        var hasAccess = await _unitOfWork.Projects.HasAccessAsync(newColumn.ProjectId, userId);
        if (!hasAccess)
            throw new UnauthorizedAccessException("User has no access to this project");

        task.ColumnId = newColumnId;
        _unitOfWork.Tasks.Update(task);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(int taskId)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(taskId)
            ?? throw new ArgumentException("Task not found");

        _unitOfWork.Tasks.Delete(task);
        await _unitOfWork.SaveChangesAsync();
    }
}
