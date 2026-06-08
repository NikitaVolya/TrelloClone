

namespace Domain.Projects
{
    public class ProjectMember
    {
        public string MemberId { get; set; } = null!;

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public DateTime JoinedAt { get; set; }
    }
}
