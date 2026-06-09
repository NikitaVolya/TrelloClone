
using Domain.Boards;

namespace Domain.Tasks
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CratedAt { get; set; }
        public DateTime? Deadline { get; set; }

        public int BoardId { get; set; }
        public Board Board { get; set; }

        public int? ColumnId { get; set; }
        public Column? Column { get; set; }

        public List<TaskAssignee> Assignees { get; set; } = new List<TaskAssignee>();
        public List<TaskComment> Comments { get; set; } = new List<TaskComment>();
    }
}
