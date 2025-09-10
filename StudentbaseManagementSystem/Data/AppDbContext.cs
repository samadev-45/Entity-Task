using Microsoft.EntityFrameworkCore;
using StudentbaseManagementSystem.Models;

namespace StudentbaseManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Students)
                .WithMany(s => s.Courses)   
                .UsingEntity(j => j.ToTable("CourseStudents"));
        }
    }
}
