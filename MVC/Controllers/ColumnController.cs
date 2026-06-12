using BLL.Services.Interface;
using Domain.Boards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using System.Threading.Tasks;


namespace MVC.Controllers
{
    [Authorize]
    public class ColumnController : Controller
    {
        private readonly IColumnService _columnService;
        private readonly IBoardService _boardService;

        public ColumnController(IColumnService columnService, IBoardService boardService)
        {
            _columnService = columnService;
            _boardService = boardService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Domain.Boards.Column newColumn)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var board = await _boardService.GetBoardByIdAsync(newColumn.BoardId);
            if (board == null)
            {
                return NotFound(new { message = "Дошка не знайдена" });
            }

            await _columnService.CreateColumnAsync(newColumn.Name, newColumn.BoardId, userId);

            return RedirectToAction("Details", "Board", new { id = newColumn.BoardId });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string? name, string? hexColor)
        {
            try
            {
                Domain.Boards.Column? column = await _columnService.GetColumnByIdAsync(id);
                if (column == null)
                {
                    return NotFound(new { message = "Колонка не знайдена" });
                }

                Board? board = await _boardService.GetBoardByIdAsync(column.BoardId);
                if (board == null)
                {
                    return NotFound(new { message = "Дошка з такою колонкою не знайдена" });
                }

                if (!string.IsNullOrWhiteSpace(name))
                {
                    await _columnService.UpdateColumnAsync(id, name);
                }

                if (!string.IsNullOrWhiteSpace(hexColor))
                {
                    column.hexColor = hexColor;
                }


                return RedirectToAction("Details", "Board", new { id = board.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Помилка при редагуванні колонки", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Domain.Boards.Column? column = await _columnService.GetColumnByIdAsync(id);
            if (column == null)
            {
                return NotFound(new { message = "Колонка не знайдена" });
            }

            Board? board = await _boardService.GetBoardByIdAsync(column.BoardId);
            if (board == null)
            {
                return NotFound(new { message = "Дошка з такою колонкою не знайдена" });
            }

            await _columnService.DeleteColumnAsync(id);
            return RedirectToAction("Details", "Board", new { id = board.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Reorder(int boardId, int columnId, int newOrder)
        {
            Domain.Boards.Column? column = await _columnService.GetColumnByIdAsync(columnId);
            if (column == null)
            {
                return NotFound(new { message = "Колонка не знайдена" });
            }

            Board? board = await _boardService.GetBoardByIdAsync(boardId);
            if (board == null)
            {
                return NotFound(new { message = "Дошка з такою колонкою не знайдена" });
            }
            /*
            var oldOrder = column.Order;

            if (newOrder < oldOrder)
            {
                foreach (var col in board.Columns.Where(c => c.Order >= newOrder && c.Order < oldOrder))
                {
                    col.Order++;
                }
            }
            else if (newOrder > oldOrder)
            {
                foreach (var col in board.Columns.Where(c => c.Order > oldOrder && c.Order <= newOrder))
                {
                    col.Order--;
                }
            }

            column.Order = newOrder;*/

            return Ok(new { message = "Порядок колонок оновлено" });
        }
    }
}
