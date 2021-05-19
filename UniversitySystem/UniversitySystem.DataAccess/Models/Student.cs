using System.Collections.Generic;

namespace UniversitySystem.DataAccess.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
