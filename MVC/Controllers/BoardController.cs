using Microsoft.AspNetCore.Mvc;
using Domain.Boards;
using Domain.Projects;
using Domain.Tasks;

namespace MVC.Controllers
{
    public class BoardController : Controller
    {
        public static readonly List<Board> MockBoards = new List<Board>
        {
            new Board
            {
                Id = 1,
                Title = "Development Board",
                CreatedAt = DateTime.Now.AddDays(-10),
                ProjectId = 1,
                Columns = new List<Column>
                {
                    new Column { Id = 1, Name = "To Do", hexColor = "#FF5733", BoardId = 1 },
                    new Column { Id = 2, Name = "In Progress", hexColor = "#33FF57", BoardId = 1 },
                    new Column { Id = 3, Name = "Done", hexColor = "#3357FF", BoardId = 1 }
                },
                Tasks = new List<Domain.Tasks.Task>
                {
                    new Domain.Tasks.Task { Id = 1, Name = "Task 1", Description = "Description 1", ColumnId = 1, BoardId = 1, CratedAt = DateTime.Now.AddDays(-5) },
                    new Domain.Tasks.Task { Id = 2, Name = "Task 2", Description = "Description 2", ColumnId = 2, BoardId = 1, CratedAt = DateTime.Now.AddDays(-3) }
                }
            },
            new Board
            {
                Id = 2,
                Title = "Marketing Board",
                CreatedAt = DateTime.Now.AddDays(-5),
                ProjectId = 1,
                Columns = new List<Column>
                {
                    new Column { Id = 4, Name = "Backlog", hexColor = "#FFC300", BoardId = 2 },
                    new Column { Id = 5, Name = "Active", hexColor = "#DAF7A6", BoardId = 2 }
                },
                Tasks = new List<Domain.Tasks.Task>()
            }
        };

        private static readonly List<Invitation> MockInvitations = new List<Invitation>();

        public IActionResult Details(int id)
        {
            var board = MockBoards.FirstOrDefault(b => b.Id == id);
            if (board == null)
            {
                return NotFound(new { message = "Дошка не знайдена" });
            }

            ViewBag.Comments = TaskController.MockComments;

            return View(board);
        }

        [HttpPost]
        public IActionResult Create(Board newBoard)
        {
            newBoard.Id = MockBoards.Count + 1;
            newBoard.CreatedAt = DateTime.Now;
            newBoard.Columns = new List<Column>();
            newBoard.Tasks = new List<Domain.Tasks.Task>();
            MockBoards.Add(newBoard);

            var project = ProjectController.MockProjects.FirstOrDefault(p => p.Id == newBoard.ProjectId);
            if (project != null)
            {
                project.Boards.Add(newBoard);
            }

            return RedirectToAction("Detail", "Project", new { id = newBoard.ProjectId });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var board = MockBoards.FirstOrDefault(b => b.Id == id);
            if (board == null)
            {
                return NotFound(new { message = "Дошка не знайдена" });
            }

            var projectId = board.ProjectId;

            MockBoards.Remove(board);

            var project = ProjectController.MockProjects.FirstOrDefault(p => p.Id == projectId);
            if (project != null)
            {
                var projectBoard = project.Boards.FirstOrDefault(b => b.Id == id);
                if (projectBoard != null)
                {
                    project.Boards.Remove(projectBoard);
                }
            }

            return RedirectToAction("Detail", "Project", new { id = projectId });
        }

        [HttpPost]
        public IActionResult InviteUser(int boardId, string userId)
        {
            var board = MockBoards.FirstOrDefault(b => b.Id == boardId);
            if (board == null)
            {
                return NotFound(new { message = "Дошка не знайдена" });
            }

            var invitation = new Invitation
            {
                UserId = userId,
                BoardId = boardId,
                CreatedAt = DateTime.Now,
                Status = InvitationStatus.Pending
            };

            MockInvitations.Add(invitation);
            return Ok(new { message = "Запрошення надіслано", invitation });
        }
    }
}
