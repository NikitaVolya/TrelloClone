using Microsoft.AspNetCore.Mvc;
using Domain.Boards;
using BLL.Services.Interface;
using Microsoft.AspNetCore.Authorization;


namespace MVC.Controllers
{
    [Authorize]
    public class BoardController : Controller
    {
        private readonly IBoardService _boardService;
        private readonly IProjectService _projectService;

        public BoardController(IBoardService boardService, IProjectService projectService)
        {
            _boardService = boardService;
            _projectService = projectService;
        }


        public async Task<IActionResult> Details(int id)
        {
            Board? board = await _boardService.GetBoardByIdAsync(id);

            if (board == null)
            {
                return NotFound(new { message = "Дошка не знайдена" });
            }

            ViewBag.Comments = await _boardService.GetBoardTaskComments(id);
            ViewBag.ProjectMembers = await _projectService.GetProjectMembers(board.ProjectId);
            return View(board);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Board newBoard)
        {
            Board board = await _boardService.CreateBoardAsync(newBoard.Title, newBoard.ProjectId);

            return RedirectToAction("Detail", "Project", new { id = board.ProjectId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Board? board = await _boardService.GetBoardByIdAsync(id);
            if (board == null)
            {
                return NotFound(new { message = "Дошка не знайдена" });
            }
            int projectId = board.ProjectId;
            await _boardService.DeleteBoardAsync(projectId);

            return RedirectToAction("Detail", "Project", new { id = projectId });
        }
    }
}
