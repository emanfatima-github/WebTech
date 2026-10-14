using Microsoft.AspNetCore.Mvc;
using AttendanceManagementSystem.Models;
using AttendanceManagementSystem.Repositories;

namespace AttendanceManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentRepository _repository;

        public StudentController(StudentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _repository.GetAllAsync();
            return View(students);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid)
                return View(student);

            await _repository.AddAsync(student);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var student = await _repository.GetByIdAsync(id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.StudentId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(student);

            var existingStudent = await _repository.GetByIdAsync(id);

            if (existingStudent == null)
                return NotFound();

            await _repository.UpdateAsync(student);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _repository.GetByIdAsync(id);

            if (student == null)
                return NotFound();

            await _repository.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
