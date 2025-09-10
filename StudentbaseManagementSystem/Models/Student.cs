namespace StudentbaseManagementSystem.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public List<Course> Courses { get; set; } = new List<Course>();
    }
}
