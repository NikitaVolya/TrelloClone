using Domain.Projects;
using Microsoft.AspNetCore.Mvc;
using BLL.Services.Interface;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Domain.Common;



namespace MVC.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        private readonly IInvitationService _invitationService;

        public ProjectController(IProjectService projectService, IUserService userService, IInvitationService invitationService)
        {
            _projectService = projectService;
            _userService = userService;
            _invitationService = invitationService;
        }

        public async Task<ActionResult> AllProjects()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<Project> projects = await _projectService.GetUserProjectsAsync(userId);

            return View("Index", projects);
        }

        public async Task<ActionResult> Index()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<Project> projects = await _projectService.GetOwnerProjectsAsync(userId);

            return View(projects);
        }

        public async Task<ActionResult> Detail(int id) { 
            Project? project = await _projectService.GetByIdAsync(id);
            if (project == null) { 
                return NotFound( new{ message = "Проект не найдено" });
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "user1";
            ViewBag.IsOwner = project.OwnerId == currentUserId;
            ViewBag.CurrentUserId = currentUserId;

            return View(project);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Project newProject) {

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _projectService.CreateAsync(newProject.Title, newProject.Description, userId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Update(int id, string title, string description)
        {
            await _projectService.UpdateAsync(id, title, description);
            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        public async Task<ActionResult> AddMember(int projectId, string userId)
        {
            Project? project = await _projectService.GetByIdAsync(projectId);
            if (project == null)
            {
                return NotFound(new { message = "Проект не знайдено" });
            }

            if (!project.Members.Any(m => m.MemberId == userId))
            {
                project.Members.Add(new ProjectMember
                {
                    MemberId = userId,
                    ProjectId = projectId,
                    JoinedAt = DateTime.Now
                });
            }

            return RedirectToAction("Detail", new { id = projectId });
        }

        [HttpPost]
        public async Task<ActionResult> RemoveMember(int projectId, string userId)
        {
            Project? project = await _projectService.GetByIdAsync(projectId);
            if (project == null)
            {
                return NotFound(new { message = "Проект не знайдено" });
            }
            
            await _projectService.RemoveMemberAsync(projectId, userId);

            return RedirectToAction("Detail", new { id = projectId });
        }

        [HttpPost]
        public async Task<ActionResult> SendInvitation(int projectId, string userEmail)
        {
            Project? project = await _projectService.GetByIdAsync(projectId);
            if (project == null)
            {
                return NotFound(new { message = "Проект не знайдено" });
            }

            ApplicationUser? user = await _userService.GetByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound(new { message = "Користувач з такою поштою не існує" });
            }

            await _invitationService.InviteUserAsync(projectId, project.OwnerId, user.Id);

            return RedirectToAction("Detail", new { id = projectId });
        }

        public async Task<ActionResult> Delete(int id)
        {

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _projectService.DeleteAsync(id, userId);
            return RedirectToAction("Index");
        }
    }
}
