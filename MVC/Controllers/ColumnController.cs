using Microsoft.AspNetCore.Mvc;
using Domain.Boards;

namespace MVC.Controllers
{

    public class ColumnController : Controller
    {

        [HttpPost]
        public IActionResult Create(Column newColumn)
        {
            var board = BoardController.MockBoards.FirstOrDefault(b => b.Id == newColumn.BoardId);
            if (board == null)
            {
                return NotFound(new { message = "Дошка не знайдена" });
            }

            var allColumns = BoardController.MockBoards.SelectMany(b => b.Columns).ToList();
            newColumn.Id = allColumns.Any() ? allColumns.Max(c => c.Id) + 1 : 1;
            board.Columns.Add(newColumn);

            return RedirectToAction("Details", "Board", new { id = newColumn.BoardId });
        }

        [HttpPost]
        public IActionResult Edit(int id, string? name, string? hexColor)
        {
            try
            {
                var board = BoardController.MockBoards.FirstOrDefault(b => b.Columns.Any(c => c.Id == id));
                if (board == null)
                {
                    return NotFound(new { message = "Дошка з такою колонкою не знайдена" });
                }

                var column = board.Columns.FirstOrDefault(c => c.Id == id);
                if (column == null)
                {
                    return NotFound(new { message = "Колонка не знайдена" });
                }

                if (!string.IsNullOrWhiteSpace(name))
                {
                    column.Name = name;
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
        public IActionResult Delete(int id)
        {
            var board = BoardController.MockBoards.FirstOrDefault(b => b.Columns.Any(c => c.Id == id));
            if (board == null)
            {
                return NotFound(new { message = "Дошка з такою колонкою не знайдена" });
            }

            var column = board.Columns.FirstOrDefault(c => c.Id == id);
            if (column != null)
            {
                board.Columns.Remove(column);
                return RedirectToAction("Details", "Board", new { id = board.Id });
            }
            return BadRequest(new { message = "Колонка не знайдена" });
        }
    }
}
