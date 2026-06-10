

using System.ComponentModel.DataAnnotations;

namespace Domain.Boards
{
    public class Column
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        [RegularExpression("^#([A-Fa-f0-9]{6})$")]
        [Required]
        public string hexColor { get; set; } = "#FFFFFF";

        public int BoardId { get; set; }
        public Board Board { get; set; }

        public int Order { get; set; }

        public List<Tasks.Task> Tasks { get; set; } = new List<Tasks.Task>();
    }
}
