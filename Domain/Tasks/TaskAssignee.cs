
using Domain.Common;

namespace Domain.Tasks
{
    public class TaskAssignee
    {
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; }

        public int TaskId { get; set; }
        public Task Task { get; set; }
    }
}
