using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentbaseManagementSystem.Data;
using StudentbaseManagementSystem.Models;

namespace StudentbaseManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // Get All Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // Get Student by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();
            return student;
        }

        // Add Student
        [HttpPost]
        public async Task<ActionResult<Student>> AddStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, student);
        }

        // Update Student
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, Student student)
        {
            if (id != student.StudentId) return BadRequest();

            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete Student
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Search Students (by name or email)
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Student>>> SearchStudents(string? name, string? email)
        {
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(s => s.StudentName.Contains(name));

            if (!string.IsNullOrEmpty(email))
                query = query.Where(s => s.StudentEmail.Contains(email));

            return await query.ToListAsync();
        }
    }
}
