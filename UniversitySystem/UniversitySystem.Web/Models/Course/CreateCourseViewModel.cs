using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Web.Models.Course
{
    public class CreateCourseViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
