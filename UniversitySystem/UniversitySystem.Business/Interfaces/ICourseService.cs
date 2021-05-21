using System.Collections.Generic;
using System.Threading.Tasks;
using UniversitySystem.Business.Models.Course;

namespace UniversitySystem.Business.Interfaces
{
    public interface ICourseService
    {
        Task<GetManageCourseResponse> GetAsync(int page, int pageSize);

        Task<GetCourseResponse> GetByIdAsync(int courseId);

        Task CreateAsync(CreateCourseRequest request);

        Task UpdateAsync(UpdateCourseRequest request);

        Task DeleteAsync(int courseId);
    }
}
