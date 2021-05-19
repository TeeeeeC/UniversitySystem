using System.Collections.Generic;

namespace UniversitySystem.DataAccess.Models
{
    public class Course 
    {
        public int CourseId { get; set; }

        public string Name { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
