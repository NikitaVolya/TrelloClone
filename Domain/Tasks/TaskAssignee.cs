
namespace Domain.Tasks
{
    public class TaskAssignee
    {
        public string UserId { get; set; } = null!;
        public int TaskId { get; set; }
        public Task Task { get; set; }
    }
}
