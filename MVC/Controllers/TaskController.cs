using Microsoft.AspNetCore.Mvc;
using Domain.Tasks;
using Domain.Boards;

namespace MVC.Controllers
{
    public class TaskController : Controller
    {
        private static readonly List<Domain.Tasks.Task> MockTasks = new List<Domain.Tasks.Task>
        {
            new Domain.Tasks.Task
            {
                Id = 1,
                Name = "Implement authentication",
                Description = "Add user authentication with JWT tokens",
                CratedAt = DateTime.Now.AddDays(-5),
                Deadline = DateTime.Now.AddDays(7),
                BoardId = 1,
                ColumnId = 1,
                Assignees = new List<TaskAssignee>
                {
                    new TaskAssignee { UserId = "user1", TaskId = 1 }
                }
            },
            new Domain.Tasks.Task
            {
                Id = 2,
                Name = "Design dashboard",
                Description = "Create mockups for the main dashboard",
                CratedAt = DateTime.Now.AddDays(-3),
                Deadline = DateTime.Now.AddDays(5),
                BoardId = 1,
                ColumnId = 2,
                Assignees = new List<TaskAssignee>()
            },
            new Domain.Tasks.Task
            {
                Id = 3,
                Name = "Write documentation",
                Description = "Document API endpoints",
                CratedAt = DateTime.Now.AddDays(-1),
                Deadline = null,
                BoardId = 1,
                ColumnId = 1,
                Assignees = new List<TaskAssignee>()
            }
        };

        public static readonly List<TaskComment> MockComments = new List<TaskComment>
        {
            new TaskComment { Id = 1, Text = "Started working on this", SenderId = "user1", CreatedAt = DateTime.Now.AddDays(-2) },
            new TaskComment { Id = 2, Text = "Need clarification on requirements", SenderId = "user2", CreatedAt = DateTime.Now.AddDays(-1) }
        };

        public IActionResult Details(int id)
        {
            var task = MockTasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound(new { message = "Таска не знайдена" });
            }

            var taskComments = MockComments.Where(c => c.TaskId == id).ToList();
            ViewBag.Comments = taskComments;

            return View(task);
        }

        [HttpPost]
        public IActionResult Create(Domain.Tasks.Task newTask)
        {
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }

            newTask.Id = MockTasks.Count + 1;
            newTask.CratedAt = DateTime.Now;

            if (newTask.Assignees == null)
            {
                newTask.Assignees = new List<TaskAssignee>();
            }

            MockTasks.Add(newTask);

            var boardTask = BoardController.MockBoards.FirstOrDefault(b => b.Id == newTask.BoardId);
            if (boardTask != null)
            {
                boardTask.Tasks.Add(newTask);
            }

            return RedirectToAction("Details", "Board", new { id = newTask.BoardId });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var task = MockTasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound(new { message = "Таска не знайдена" });
            }

            var boardId = task.BoardId;

            MockTasks.Remove(task);

            var board = BoardController.MockBoards.FirstOrDefault(b => b.Id == boardId);
            if (board != null)
            {
                var boardTask = board.Tasks.FirstOrDefault(t => t.Id == id);
                if (boardTask != null)
                {
                    board.Tasks.Remove(boardTask);
                }
            }

            return RedirectToAction("Details", "Board", new { id = boardId });
        }

        [HttpPost]
        public IActionResult Move(int id, int columnId)
        {
            var task = MockTasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound(new { message = "Таска не знайдена" });
            }

            task.ColumnId = columnId;

            var board = BoardController.MockBoards.FirstOrDefault(b => b.Id == task.BoardId);
            if (board != null)
            {
                var boardTask = board.Tasks.FirstOrDefault(t => t.Id == id);
                if (boardTask != null)
                {
                    boardTask.ColumnId = columnId;
                }
            }

            return Ok(new { message = "Таска переміщена", task });
        }

        [HttpPost]
        public IActionResult AddComment(int taskId, string text, string senderId)
        {
            var task = MockTasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                return NotFound(new { message = "Таска не знайдена" });
            }

            var comment = new TaskComment
            {
                Id = MockComments.Count + 1,
                Text = text,
                SenderId = senderId,
                CreatedAt = DateTime.Now,
                TaskId = taskId
            };

            MockComments.Add(comment);
            return RedirectToAction("Details", "Board", new { id = task.BoardId });
        }

        [HttpPost]
        public IActionResult AssignUser(int taskId, string userId)
        {
            var board = BoardController.MockBoards.FirstOrDefault(b => b.Tasks.Any(t => t.Id == taskId));
            if (board == null)
            {
                return NotFound(new { message = "Дошка не знайдена" });
            }

            var task = board.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                return NotFound(new { message = "Таска не знайдена" });
            }

            if (!task.Assignees.Any(a => a.UserId == userId))
            {
                var assignee = new TaskAssignee
                {
                    UserId = userId,
                    TaskId = taskId
                };

                task.Assignees.Add(assignee);
            }

            return RedirectToAction("Details", "Board", new { id = board.Id });
        }

        [HttpPost]
        public IActionResult RemoveAssignee(int taskId, string userId)
        {
            var board = BoardController.MockBoards.FirstOrDefault(b => b.Tasks.Any(t => t.Id == taskId));
            if (board == null)
            {
                return NotFound(new { message = "Дошка не знайдена" });
            }

            var task = board.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                return NotFound(new { message = "Таска не знайдена" });
            }

            var assignee = task.Assignees.FirstOrDefault(a => a.UserId == userId);
            if (assignee != null)
            {
                task.Assignees.Remove(assignee);
            }

            return RedirectToAction("Details", "Board", new { id = board.Id });
        }
    }
}
