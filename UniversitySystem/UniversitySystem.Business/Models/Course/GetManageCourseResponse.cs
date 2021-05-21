using System.Collections.Generic;

namespace UniversitySystem.Business.Models.Course
{
    public class GetManageCourseResponse
    {
        public int TotalPages { get; set; }

        public IEnumerable<GetCourseResponse> Courses { get; set; }
    }
}
