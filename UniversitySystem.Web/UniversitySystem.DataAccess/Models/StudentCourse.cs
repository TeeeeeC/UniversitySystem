namespace UniversitySystem.DataAccess.Models
{
    public class StudentCourse
    {
        public int CourseID { get; set; }

        public Course Course { get; set; }

        public int StudentID { get; set; }

        public Student Student { get; set; }
    }
}