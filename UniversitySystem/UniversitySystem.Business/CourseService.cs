using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniversitySystem.Business.Interfaces;
using UniversitySystem.Business.Models.Course;
using UniversitySystem.DataAccess.Models;
using UniversitySystem.DataAccess.Repositories;

namespace UniversitySystem.Business
{
    public class CourseService : ICourseService
    {
        private readonly IAsyncRepository<Course> _coursesRepository;
        private readonly IAsyncRepository<StudentCourse> _studentsCoursesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CourseService(IAsyncRepository<Course> coursesRepository,
            IAsyncRepository<StudentCourse> studentsCoursesRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _coursesRepository = coursesRepository;
            _studentsCoursesRepository = studentsCoursesRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetManageCourseResponse> GetAsync(int page, int pageSize)
        {
            var orderedCourses = (await _coursesRepository.GetAllAsync()).OrderBy(c => c.CourseId);
            var courses = orderedCourses
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new GetCourseResponse { CourseId = c.CourseId, Name = c.Name });

            return new GetManageCourseResponse
            {
                Courses = courses,
                TotalPages = (int)Math.Ceiling(orderedCourses.Count() / (double)pageSize)
            };
        }

        public async Task<GetCourseResponse> GetByIdAsync(int courseId)
        {
            var course = await _coursesRepository.GetByIdAsync(courseId);
            return new GetCourseResponse { CourseId = course.CourseId, Name = course.Name };
        }

        public async Task CreateAsync(CreateCourseRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Name))
            {
                throw new ArgumentException("Name of course is required.");
            }

            await _coursesRepository.AddAsync(new Course { Name = request.Name });
        }

        public async Task DeleteAsync(int courseId)
        {
            var entity = await _coursesRepository.GetByIdAsync(courseId);
            if (entity == null)
            {
                throw new ArgumentException($"Course with id:{courseId} does not exist.");
            }

            await _coursesRepository.DeleteAsync(entity);
        }

        public async Task UpdateAsync(UpdateCourseRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("Request should not be empty");
            }

            var entity = await _coursesRepository.GetByIdAsync(request.CourseId);
            if (entity == null)
            {
                throw new ArgumentException($"Course with id:{request.CourseId} does not exist.");
            }

            entity.Name = request.Name;
            await _coursesRepository.UpdateAsync(entity);
        }

        public async Task<bool> JoinAsync(int courseId)
        {
            var studentId = this.GetStudentId();
            var studentCourse = (await _studentsCoursesRepository.GetAsync(
                sc => sc.StudentID == studentId && sc.CourseID == courseId)).FirstOrDefault();

            if (studentCourse == null)
            {
                await _studentsCoursesRepository.AddAsync(new StudentCourse { CourseID = courseId, StudentID = studentId });
                return true;
            }

            return false;
        }

        public async Task<GetManageCourseResponse> GetAllByStudentIdAsync(int page, int pageSize)
        {
            var studentId = this.GetStudentId();
            var orderedCourseIds = (await _studentsCoursesRepository.GetAsync(sc => sc.StudentID == studentId))
                .OrderBy(sc => sc.CourseID);

            var courseIds = orderedCourseIds.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(sc => sc.CourseID);

            var orderedCourses = (await _coursesRepository.GetAsync(c => courseIds.Contains(c.CourseId)))
                .OrderBy(c => c.CourseId)
                .Select(c => new GetCourseResponse { CourseId = c.CourseId, Name = c.Name });

            return new GetManageCourseResponse
            {
                Courses = orderedCourses,
                TotalPages = (int)Math.Ceiling(orderedCourseIds.Count() / (double)pageSize)
            };
        }

        private int GetStudentId()
        {
            var nameIdentifier = _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (nameIdentifier == null)
            {
                throw new ArgumentException("Student id is not found!");
            }

            _ = int.TryParse(nameIdentifier, out int studentId);

            return studentId;
        }
    }
}
