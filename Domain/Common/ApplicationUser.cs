
using Domain.Projects;
using Domain.Tasks;
using Microsoft.AspNetCore.Identity;


namespace Domain.Common
{
    public class ApplicationUser : IdentityUser
    {
        public List<TaskComment> Comments { get; set; } = new List<TaskComment>();
        public List<Project> Projects { get; set; } = new List<Project>();
        public List<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
        public List<Invitation> Invitations { get; set; } = new List<Invitation>();
        public List<TaskAssignee> Assignees { get; set; } = new List<TaskAssignee>();
    }
}
