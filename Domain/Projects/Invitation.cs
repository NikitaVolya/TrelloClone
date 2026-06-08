
using Domain.Boards;

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
        public string UserId { get; set; } = null!;

        public int BoardId { get; set; }    
        public Board Board { get; set; }

        public DateTime CreatedAt { get; set; }

        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
    }
}
