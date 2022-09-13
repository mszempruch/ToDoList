using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ToDoContext context;

        public ProjectController (ToDoContext context)
        {
            this.context = context;
        }

        // GET /
        public async Task<ActionResult> Index()
        {
            IQueryable<ProjectList> items = from i in context.ProjectLists orderby i.Id select i;

            List<ProjectList> projectList = await items.ToListAsync();

            return View(projectList);
        }
        [HttpGet]

        public async Task<IActionResult> Index(string Projectsearch, string sortingproject)
        {
            ViewData["Getprojectdetails"] = Projectsearch;

            ViewData["Sortingproject"] = string.IsNullOrEmpty(sortingproject) ? "Project" : "";

            var projectquery = from x in context.ProjectLists select x;

            switch (sortingproject)
            {
                case "Project":
                    projectquery = projectquery.OrderBy(x => x.Project);
                    break;
                default:
                    projectquery = projectquery.OrderByDescending(x => x.Project);
                    break;

            }

            if (!String.IsNullOrEmpty(Projectsearch))
            {
                projectquery = projectquery.Where(x => x.Project.Contains(Projectsearch));
            }
            return View(await projectquery.AsNoTracking().ToListAsync());

        }

        // GET /project/create
        public IActionResult Create() => View();

        // POST /project/crete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProjectList item)
        {
            if (ModelState.IsValid)
            {
                context.Add(item);
                await context.SaveChangesAsync();

                TempData["Sukces"] = "Projekt został dodany do listy!";

                return RedirectToAction("Index");
            }


            return View(item);
        }
        // GET /project/edit/5
        public async Task<ActionResult> Edit(int id)
        {
            ProjectList item = await context.ProjectLists.FindAsync(id);
            if (item == null)
            {
                return NotFound(item);
            }

            return View(item);
        }
        // POST /project/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProjectList item)
        {
            if (ModelState.IsValid)
            {
                context.Update(item);
                await context.SaveChangesAsync();

                TempData["Sukces"] = "Projekt został zaktualizowany!";

                return RedirectToAction("Index");
            }


            return View(item);
        }
        // GET /project/delete/5
        public async Task<ActionResult> Delete(int id)
        {
            ProjectList item = await context.ProjectLists.FindAsync(id);
            if (item == null)
            {
                TempData["Błąd"] = "Projekt nie istnieje!";
            }
            else
            {
                context.ProjectLists.Remove(item);
                await context.SaveChangesAsync();

                TempData["Sukces"] = "Projekt został usunięty!";

            }

            return RedirectToAction("Index");
        }
    }
}
