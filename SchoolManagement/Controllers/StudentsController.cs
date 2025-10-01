using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;
using System.Linq;

namespace SchoolManagement.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public IActionResult Index()
        {
            var students = _context.Students
                .Include(s => s.School)
                .Include(s => s.SchoolClass)
                .Include(s => s.StudentSubjects)
                    .ThenInclude(ss => ss.Subject)
                .ToList();

            return View(students);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["SchoolId"] = new SelectList(_context.Schools, "Id", "Name");
            ViewData["SchoolClassId"] = new SelectList(_context.SchoolClasses, "Id", "Name");
            ViewData["SubjectIds"] = new MultiSelectList(_context.Subjects, "Id", "Name");
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student, int[] SubjectIds)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                _context.SaveChanges();

                foreach (var subId in SubjectIds)
                {
                    _context.StudentSubjects.Add(new StudentSubject
                    {
                        StudentId = student.Id,
                        SubjectId = subId
                    });
                }
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewData["SchoolId"] = new SelectList(_context.Schools, "Id", "Name", student.SchoolId);
            ViewData["SchoolClassId"] = new SelectList(_context.SchoolClasses, "Id", "Name", student.SchoolClassId);
            ViewData["SubjectIds"] = new MultiSelectList(_context.Subjects, "Id", "Name", SubjectIds);

            return View(student);
        }

        // GET: Students/Edit/5
        public IActionResult Edit(int id)
        {
            var student = _context.Students
                .Include(s => s.StudentSubjects)
                .FirstOrDefault(s => s.Id == id);

            if (student == null) return NotFound();

            ViewData["SchoolId"] = new SelectList(_context.Schools, "Id", "Name", student.SchoolId);
            ViewData["SchoolClassId"] = new SelectList(_context.SchoolClasses, "Id", "Name", student.SchoolClassId);

            var selectedSubjects = student.StudentSubjects.Select(ss => ss.SubjectId).ToArray();
            ViewData["SubjectIds"] = new MultiSelectList(_context.Subjects, "Id", "Name", selectedSubjects);

            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student student, int[] SubjectIds)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Update(student);
                _context.SaveChanges();

                // Remove old subjects
                var oldSubjects = _context.StudentSubjects.Where(ss => ss.StudentId == student.Id);
                _context.StudentSubjects.RemoveRange(oldSubjects);

                // Add new subjects
                foreach (var subId in SubjectIds)
                {
                    _context.StudentSubjects.Add(new StudentSubject
                    {
                        StudentId = student.Id,
                        SubjectId = subId
                    });
                }
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewData["SchoolId"] = new SelectList(_context.Schools, "Id", "Name", student.SchoolId);
            ViewData["SchoolClassId"] = new SelectList(_context.SchoolClasses, "Id", "Name", student.SchoolClassId);
            ViewData["SubjectIds"] = new MultiSelectList(_context.Subjects, "Id", "Name", SubjectIds);

            return View(student);
        }

        // GET: Students/Delete/5
        public IActionResult Delete(int id)
        {
            var student = _context.Students
                .Include(s => s.School)
                .Include(s => s.SchoolClass)
                .FirstOrDefault(s => s.Id == id);

            if (student == null) return NotFound();

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = _context.Students
                .Include(s => s.StudentSubjects)
                .FirstOrDefault(s => s.Id == id);

            if (student != null)
            {
                _context.StudentSubjects.RemoveRange(student.StudentSubjects);
                _context.Students.Remove(student);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
