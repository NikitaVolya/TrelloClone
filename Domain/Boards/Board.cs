
using Domain.Projects;


namespace Domain.Boards
{
    public class Board
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public List<Column> Columns { get; set; } = new List<Column>();
        public List<Tasks.Task> Tasks { get; set; } = new List<Tasks.Task>();
    }
}
