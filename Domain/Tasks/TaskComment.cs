

using Domain.Common;

namespace Domain.Tasks
{
    public class TaskComment
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public int TaskId { get; set; }
        public Task Task { get; set; }

        public string? SenderId { get; set; }
        public ApplicationUser? Sender { get; set; }
    }
}
