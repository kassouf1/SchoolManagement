using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;    // <- must be included
using SchoolManagement.Models;  // <- must be included

namespace SchoolManagement.Controllers
{
    public class SchoolsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SchoolsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Schools
        public async Task<IActionResult> Index()
        {
            return View(await _context.Schools.ToListAsync());
        }

        // GET: Schools/Create
        public IActionResult Create() => View();

        // POST: Schools/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(School school)
        {
            if (ModelState.IsValid)
            {
                _context.Schools.Add(school);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(school);
        }
    }
}
