
namespace Domain.Projects
{
    public enum InvitationStatus
    {
        Pending = 0,
        Approved = 1,
        Declined = 2
    }

    public class Invitation
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public DateTime CreatedAt { get; set; }

        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
    }
}
