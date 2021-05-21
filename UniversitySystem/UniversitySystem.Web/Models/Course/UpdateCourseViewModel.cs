using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Web.Models.Course
{
    public class UpdateCourseViewModel
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
