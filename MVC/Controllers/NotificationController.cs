using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Domain.Projects;
using System.Security.Claims;

namespace MVC.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "user1";

            var allInvitations = ProjectController.MockProjects
                .SelectMany(p => p.Invitations.Select(i => new
                {
                    Invitation = i,
                    ProjectTitle = p.Title,
                    ProjectId = p.Id
                }))
                .Where(x => x.Invitation.UserId == currentUserId)
                .OrderByDescending(x => x.Invitation.CreatedAt)
                .ToList();

            ViewBag.CurrentUserId = currentUserId;
            return View(allInvitations);
        }

        [HttpPost]
        public IActionResult AcceptInvitation(int invitationId, int projectId)
        {
            var project = ProjectController.MockProjects.FirstOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                return NotFound(new { message = "Проект не знайдено" });
            }

            var invitation = project.Invitations.FirstOrDefault(i => i.Id == invitationId);
            if (invitation == null)
            {
                return NotFound(new { message = "Запрошення не знайдено" });
            }

            invitation.Status = InvitationStatus.Approved;

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? invitation.UserId;

            if (!project.Members.Any(m => m.MemberId == currentUserId))
            {
                project.Members.Add(new ProjectMember
                {
                    MemberId = currentUserId,
                    ProjectId = projectId,
                    JoinedAt = DateTime.Now
                });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeclineInvitation(int invitationId, int projectId)
        {
            var project = ProjectController.MockProjects.FirstOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                return NotFound(new { message = "Проект не знайдено" });
            }

            var invitation = project.Invitations.FirstOrDefault(i => i.Id == invitationId);
            if (invitation == null)
            {
                return NotFound(new { message = "Запрошення не знайдено" });
            }

            invitation.Status = InvitationStatus.Declined;

            return RedirectToAction("Index");
        }
    }
}
