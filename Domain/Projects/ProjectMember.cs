

using Domain.Common;

namespace Domain.Projects
{
    public class ProjectMember
    {
        public string MemberId { get; set; } = null!;
        public ApplicationUser Member;

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public DateTime JoinedAt { get; set; }
    }
}
