using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Domain.Projects;
using System.Security.Claims;
using BLL.Services.Interface;

namespace MVC.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly IInvitationService _invitationService;
        private readonly IProjectService _projectService;

        public NotificationController(IInvitationService invitationService, IProjectService projectService)
        {
            _invitationService = invitationService;
            _projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<Invitation> invitations = await _invitationService.GetUserInvitationsAsync(userId);

            ViewBag.CurrentUserId = userId;

            return View(invitations);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptInvitation(int invitationId, int projectId)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _invitationService.AcceptInvitationAsync(invitationId, userId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeclineInvitation(int invitationId, int projectId)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _invitationService.DeclineInvitationAsync(invitationId, userId);

            return RedirectToAction("Index");
        }
    }
}
