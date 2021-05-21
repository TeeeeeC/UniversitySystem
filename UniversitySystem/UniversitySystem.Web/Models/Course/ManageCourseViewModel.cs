using System.Collections.Generic;

namespace UniversitySystem.Web.Models.Course
{
    public class ManageCourseViewModel
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;

        public IList<CourseViewModel> Courses { get; set; }
    }
}
