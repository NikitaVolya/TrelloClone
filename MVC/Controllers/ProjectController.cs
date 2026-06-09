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
        private static readonly List<Project> MockProjects = new List<Project>
        {
            new Project { Id = 1, Title = "Project A", Description = "Description of Project A", Boards = new List<Board>(){ new Board { Id = 1, Title = "Board 1"  },{ new Board { Id = 2, Title = "Board 2" } } } },
            new Project { Id = 2, Title = "Project B", Description = "Description of Project B", Boards = new List<Board>(){ new Board { Id = 2, Title = "Board 2" }, new Board { Id = 3, Title = "Board 3" } } },
            new Project { Id = 3, Title = "Project C", Description = "Description of Project C", Boards = new List<Board>(){ new Board { Id = 3, Title = "Board 3" }, new Board { Id = 4, Title = "Board 4" } } }
        };
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

        public ActionResult Create(Project newProject) {
            newProject.Id = MockProjects.Count + 1;
            MockProjects.Add(newProject);
            return RedirectToAction("Index");
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
