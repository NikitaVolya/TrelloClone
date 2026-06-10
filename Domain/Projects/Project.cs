

using Domain.Boards;

namespace Domain.Projects
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public string OwnerId { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public List<Board> Boards { get; set; } = new List<Board>();
        public List<ProjectMember> Members { get; set; } = new List<ProjectMember>();
        public List<Invitation> Invitations { get; set; } = new List<Invitation>();
    }
}
