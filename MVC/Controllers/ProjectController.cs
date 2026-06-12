using Domain.Projects;
using Microsoft.AspNetCore.Mvc;
using BLL.Services.Interface;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;



namespace MVC.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IInvitationService _invitationService;

        public ProjectController(IProjectService projectService, IInvitationService invitationService)
        {
            _projectService = projectService;
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

            List<Project> projects = await _projectService.GetUserProjectsAsync(userId);

            return View(projects);
        }

        public async Task<ActionResult> Detail(int id) { 
            Project? project = await _projectService.GetByIdAsync(id);
            if (project == null) { 
                return NotFound( new{ message = "Проект не найдено" });
            }
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

            var member = project.Members.FirstOrDefault(m => m.MemberId == userId);
            if (member != null)
            {
                project.Members.Remove(member);
            }

            return RedirectToAction("Detail", new { id = projectId });
        }

        [HttpPost]
        public async Task<ActionResult> SendInvitation(int projectId, string userId)
        {
            Project? project = await _projectService.GetByIdAsync(projectId);
            if (project == null)
            {
                return NotFound(new { message = "Проект не знайдено" });
            }

            await _invitationService.InviteUserAsync(projectId, project.OwnerId, userId);

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
