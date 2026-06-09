

namespace Domain.Tasks
{
    public class TaskComment
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public string SenderId { get; set; } = null!;
        public int TaskId { get; set; }
    }
}
