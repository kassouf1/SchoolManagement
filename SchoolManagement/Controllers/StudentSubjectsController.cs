using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Data;
using SchoolManagement.Models;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace SchoolManagement.Controllers
{
    public class StudentSubjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentSubjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StudentSubjects
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.StudentSubjects.Include(s => s.Student).Include(s => s.Subject);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: StudentSubjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentSubject = await _context.StudentSubjects
                .Include(s => s.Student)
                .Include(s => s.Subject)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (studentSubject == null)
            {
                return NotFound();
            }

            return View(studentSubject);
        }

        // GET: StudentSubjects/Create
        public IActionResult Create()
        {
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Id");
            return View();
        }

        // POST: StudentSubjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,SubjectId")] StudentSubject studentSubject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentSubject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", studentSubject.StudentId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Id", studentSubject.SubjectId);
            return View(studentSubject);
        }

        // GET: StudentSubjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentSubject = await _context.StudentSubjects.FindAsync(id);
            if (studentSubject == null)
            {
                return NotFound();
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", studentSubject.StudentId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Id", studentSubject.SubjectId);
            return View(studentSubject);
        }

        // POST: StudentSubjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,SubjectId")] StudentSubject studentSubject)
        {
            if (id != studentSubject.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentSubject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentSubjectExists(studentSubject.StudentId))
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
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", studentSubject.StudentId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Id", studentSubject.SubjectId);
            return View(studentSubject);
        }

        // GET: StudentSubjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentSubject = await _context.StudentSubjects
                .Include(s => s.Student)
                .Include(s => s.Subject)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (studentSubject == null)
            {
                return NotFound();
            }

            return View(studentSubject);
        }

        // POST: StudentSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentSubject = await _context.StudentSubjects.FindAsync(id);
            if (studentSubject != null)
            {
                _context.StudentSubjects.Remove(studentSubject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentSubjectExists(int id)
        {
            return _context.StudentSubjects.Any(e => e.StudentId == id);
        }
    }
}
