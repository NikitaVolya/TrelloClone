using Domain.Projects;
using Domain.Boards;
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

        public static readonly List<Project> MockProjects = new List<Project>
        {
            new Project
            {
                Id = 1,
                Title = "Project A",
                Description = "Description of Project A",
                OwnerId = "user1",
                CreatedAt = DateTime.Now.AddDays(-10),
                Boards = new List<Board>()
                {
                    new Board { Id = 1, Title = "Board 1", CreatedAt = DateTime.Now.AddDays(-8) },
                    new Board { Id = 2, Title = "Board 2", CreatedAt = DateTime.Now.AddDays(-5) }
                },
                Members = new List<ProjectMember>()
                {
                    new ProjectMember { MemberId = "user2", ProjectId = 1, JoinedAt = DateTime.Now.AddDays(-9) },
                    new ProjectMember { MemberId = "user3", ProjectId = 1, JoinedAt = DateTime.Now.AddDays(-5) }
                },
                Invitations = new List<Invitation>()
                {
                    new Invitation { Id = 1, UserId = "user4", ProjectId = 1, CreatedAt = DateTime.Now.AddDays(-2), Status = InvitationStatus.Pending },
                    new Invitation { Id = 2, UserId = "user5", ProjectId = 1, CreatedAt = DateTime.Now.AddDays(-3), Status = InvitationStatus.Declined }
                }
            },
            new Project
            {
                Id = 2,
                Title = "Project B",
                Description = "Description of Project B",
                OwnerId = "user1",
                CreatedAt = DateTime.Now.AddDays(-2),
                Boards = new List<Board>()
                {
                    new Board { Id = 5, Title = "Board 5", CreatedAt = DateTime.Now.AddDays(-1) }
                },
                Members = new List<ProjectMember>(),
                Invitations = new List<Invitation>()
            }
        };


        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
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

        public ActionResult Detail(int id) {
            var project = MockProjects.FirstOrDefault(p => p.Id == id);
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
        public ActionResult AddMember(int projectId, string userId)
        {
            var project = MockProjects.FirstOrDefault(p => p.Id == projectId);
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
        public ActionResult RemoveMember(int projectId, string userId)
        {
            var project = MockProjects.FirstOrDefault(p => p.Id == projectId);
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
        public ActionResult SendInvitation(int projectId, string userId)
        {
            var project = MockProjects.FirstOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                return NotFound(new { message = "Проект не знайдено" });
            }

            if (!project.Invitations.Any(i => i.UserId == userId && i.Status == InvitationStatus.Pending))
            {
                var newInvitation = new Invitation
                {
                    Id = MockProjects.SelectMany(p => p.Invitations).Count() + 1,
                    UserId = userId,
                    ProjectId = projectId,
                    CreatedAt = DateTime.Now,
                    Status = InvitationStatus.Pending
                };
                project.Invitations.Add(newInvitation);
            }

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
