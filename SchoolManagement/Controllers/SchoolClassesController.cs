using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    public class SchoolClassesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SchoolClassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SchoolClasses
        public IActionResult Index()
        {
            var classes = _context.SchoolClasses.Include(c => c.School).ToList();
            return View(classes);
        }

        // GET: SchoolClasses/Create
        public IActionResult Create()
        {
            ViewData["SchoolId"] = new SelectList(_context.Schools, "Id", "Name");
            return View();
        }

        // POST: SchoolClasses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,SchoolId")] SchoolClass schoolClass)
        {
            if (ModelState.IsValid)
            {
                _context.SchoolClasses.Add(schoolClass);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewData["SchoolId"] = new SelectList(_context.Schools, "Id", "Name", schoolClass.SchoolId);
            return View(schoolClass);
        }

        // GET: SchoolClasses/Edit/5
        public IActionResult Edit(int id)
        {
            var schoolClass = _context.SchoolClasses.Find(id);
            if (schoolClass == null) return NotFound();

            ViewData["SchoolId"] = new SelectList(_context.Schools, "Id", "Name", schoolClass.SchoolId);
            return View(schoolClass);
        }

        // POST: SchoolClasses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,SchoolId")] SchoolClass schoolClass)
        {
            if (id != schoolClass.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(schoolClass);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewData["SchoolId"] = new SelectList(_context.Schools, "Id", "Name", schoolClass.SchoolId);
            return View(schoolClass);
        }

        // GET: SchoolClasses/Delete/5
        public IActionResult Delete(int id)
        {
            var schoolClass = _context.SchoolClasses.Include(c => c.School)
                                .FirstOrDefault(c => c.Id == id);
            if (schoolClass == null) return NotFound();

            return View(schoolClass);
        }

        // POST: SchoolClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var schoolClass = _context.SchoolClasses.Find(id);
            if (schoolClass != null)
            {
                _context.SchoolClasses.Remove(schoolClass);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
