using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Projects;
using Domain.Boards;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;


namespace MVC.Controllers
{
    public class ProjectController : Controller
    {
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

        public ActionResult AllProjects()
        {
            return View("Index", MockProjects);
        }

        public ActionResult Index()
        {
            
            return View(MockProjects);
        }

        public ActionResult Detail(int id) { 
            var project = MockProjects.FirstOrDefault(p => p.Id == id);
            if (project == null) { 
                return NotFound( new{ message = "Проект не найдено" });
            }
            return View(project);
        }

        [HttpPost]
        public ActionResult Create(Project newProject) {
            newProject.Id = MockProjects.Count + 1;
            newProject.CreatedAt = DateTime.Now;
            newProject.OwnerId = "user1";
            newProject.Boards = new List<Board>();
            newProject.Members = new List<ProjectMember>();
            newProject.Invitations = new List<Invitation>();
            MockProjects.Add(newProject);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Update(int id, string title, string description)
        {
            var project = MockProjects.FirstOrDefault(p => p.Id == id);
            if (project == null)
            {
                return NotFound(new { message = "Проект не знайдено" });
            }

            project.Title = title;
            project.Description = description;

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

        public ActionResult Delete(int id)
        {
            var project = MockProjects.FirstOrDefault(p => p.Id == id);
                        if (project != null)
            {
                MockProjects.Remove(project);
            }
            return RedirectToAction("Index");
        }
    }
}
