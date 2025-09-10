using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentbaseManagementSystem.Data;
using StudentbaseManagementSystem.Models;

namespace StudentbaseManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        // Get All Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.Include(c => c.Students).ToListAsync();
        }

        // Get Course by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses.Include(c => c.Students)
                                               .FirstOrDefaultAsync(c => c.CourseId == id);
            if (course == null) return NotFound();
            return course;
        }

        // Add Course
        [HttpPost]
        public async Task<ActionResult<Course>> AddCourse(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCourse), new { id = course.CourseId }, course);
        }

        // Update Course
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, Course course)
        {
            if (id != course.CourseId) return BadRequest();

            _context.Entry(course).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete Course
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Search Courses by name or teacher
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Course>>> SearchCourses(string? name, string? teacher)
        {
            var query = _context.Courses.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(c => c.Name.Contains(name));

            if (!string.IsNullOrEmpty(teacher))
                query = query.Where(c => c.Teacher.Contains(teacher));

            return await query.ToListAsync();
        }
    }
}
