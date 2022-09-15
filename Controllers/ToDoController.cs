using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ToDoContext _context;

        public ToDoController(ToDoContext context)
        {
            _context = context;
        }

        // GET: ToDo
        public async Task<IActionResult> Index()
        {
            var toDoContext = _context.TodoLists.Include(t => t.Projectlist);
            return View(await toDoContext.ToListAsync());
        }
        [HttpGet]

        public async Task<IActionResult> Index(string Tasksearch, string sortingtask, string sortingproject)
        {
            ViewData["Gettodolistdetails"] = Tasksearch;

            ViewData["Sortingtask"] = string.IsNullOrEmpty(sortingtask) ? "Content" : "";

            ViewData["Sortingproject"] = string.IsNullOrEmpty(sortingproject) ? "Project" : "";

            var taskquery = from x in _context.TodoLists.Include(t => t.Projectlist) select x;

            var projectquery = from t in _context.ProjectLists select t;

            switch (sortingtask)
            {
                case "Content":
                    taskquery = taskquery.OrderBy(x => x.Content);
                    break;
                default:
                    taskquery = taskquery.OrderByDescending(x => x.Content);
                    break;
            }
            
            switch (sortingproject)
            {
                case "Project":
                    projectquery = projectquery.OrderBy(t => t.Project);
                    break;
                default:
                    projectquery = projectquery.OrderByDescending(t => t.Project);
                    break;
            }


            if (!String.IsNullOrEmpty(Tasksearch))
            {
                taskquery = taskquery.Where(x => x.Content.Contains(Tasksearch));
                projectquery = projectquery.Where(t => t.Project.Contains(Tasksearch));
            }

            return View(await taskquery.AsNoTracking().ToListAsync()); 
        }

        // GET: ToDo/Create
        public IActionResult Create()
        {
            ViewData["ProjectListId"] = new SelectList(_context.ProjectLists, "ProjectListId", "Project");
            return View();
        }

        // POST: ToDo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TodoListId,Content,ProjectListId")] TodoList todoList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(todoList);
                await _context.SaveChangesAsync();
                TempData["Sukces"] = "Zadanie zostało dodane do listy!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectListId"] = new SelectList(_context.ProjectLists, "ProjectListId", "Project", todoList.ProjectListId);
            return View(todoList);
        }

        // GET: ToDo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoList = await _context.TodoLists.FindAsync(id);
            if (todoList == null)
            {
                return NotFound();
            }
            ViewData["ProjectListId"] = new SelectList(_context.ProjectLists, "ProjectListId", "Project", todoList.ProjectListId);
            return View(todoList);
        }

        // POST: ToDo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TodoListId,Content,ProjectListId")] TodoList todoList)
        {
            if (id != todoList.TodoListId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todoList);
                    await _context.SaveChangesAsync();
                    TempData["Sukces"] = "Zadanie zostało zaktualizowane!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoListExists(todoList.TodoListId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectListId"] = new SelectList(_context.ProjectLists, "ProjectListId", "Project", todoList.ProjectListId);
            return View(todoList);
        }

        // GET: ToDo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["Błąd"] = "Zadanie nie istnieje!";
            }

            var todoList = await _context.TodoLists
                .Include(t => t.Projectlist)
                .FirstOrDefaultAsync(m => m.TodoListId == id);
            if (todoList == null)
            {
                return NotFound();
            }

            return View(todoList);
        }

        // POST: ToDo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todoList = await _context.TodoLists.FindAsync(id);
            _context.TodoLists.Remove(todoList);
            await _context.SaveChangesAsync();
            TempData["Sukces"] = "Zadanie zostało usunięte!";
            return RedirectToAction(nameof(Index));
        }

        private bool TodoListExists(int id)
        {
            return _context.TodoLists.Any(e => e.TodoListId == id);
        }
    }
}
