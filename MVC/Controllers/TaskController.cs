using Microsoft.AspNetCore.Mvc;
using Domain.Tasks;
using Domain.Boards;
using BLL.Services.Interface;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IBoardService _boardService;
        private readonly IColumnService _columnService;
        
        public TaskController(ITaskService taskService, IBoardService boardService, IColumnService columnService)
        {
            _taskService = taskService;
            _boardService = boardService;
            _columnService = columnService;
        }
        

        public async Task<IActionResult> Details(int id)
        {
            Domain.Tasks.Task? task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound(new { message = "Таска не знайдена" });
            }

            ViewBag.Comments = task.Comments;

            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Domain.Tasks.Task newTask)
        {
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Column> columns = await _columnService.GetColumnsForBoardAsync(newTask.BoardId);
            Column? column = columns.OrderBy(cl => cl.Order).FirstOrDefault();

            Domain.Tasks.Task? task = await _taskService.CreateTaskAsync(newTask.Name, newTask.Description, newTask.BoardId, column?.Id, userId);
            await _taskService.SetTaskDeadline(task.Id, newTask.Deadline);
            await _taskService.MoveTaskAsync(task.Id, newTask.ColumnId, userId);

            return RedirectToAction("Details", "Board", new { id = task.BoardId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Domain.Tasks.Task? task = await _taskService.GetTaskByIdAsync(id);

            if (task == null)
                return NotFound("Task not found");

            int boardId = task.BoardId;

            await _taskService.DeleteTaskAsync(id);

            return RedirectToAction("Details", "Board", new { id = boardId });
        }

        [HttpPost]
        public async Task<IActionResult> Move(int id, int columnId)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _taskService.MoveTaskAsync(id, columnId, userId);

            Domain.Tasks.Task? task = await _taskService.GetTaskByIdAsync(id);

            return Ok(new { message = "Таска переміщена" });
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int taskId, string text)
        {

            Domain.Tasks.Task? task = await _taskService.GetTaskByIdAsync(taskId);
            if (task == null)
            {
                return NotFound(new { message = "Таска не знайдена" });
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _taskService.LeaveTaskComment(taskId, userId, text);

            return RedirectToAction("Details", "Board", new { id = task.BoardId });
        }

        [HttpPost]
        public async Task<IActionResult> AssignUser(int taskId, string userId)
        {
            Domain.Tasks.Task? task = await _taskService.GetTaskByIdAsync(taskId);
            if (task == null)
            {
                return NotFound(new { message = "Таска не знайдена" });
            }
            
            Domain.Tasks.TaskAssignee assignee = await _taskService.AssigneUser(taskId, userId);

            return RedirectToAction("Details", "Board", new { id = task.BoardId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAssignee(int taskId, string userId)
        {
            var task = await _taskService.GetTaskByIdAsync(taskId);
            if (task == null)
            {
                return NotFound(new { message = "Таска не знайдена" });
            }

            await _taskService.DeassigneUser(taskId, userId);

            return RedirectToAction("Details", "Board", new { id = task.BoardId });
        }
    }
}
