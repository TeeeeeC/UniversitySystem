using System;
using System.Collections.Generic;
using System.Linq;
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

        public CourseService(IAsyncRepository<Course> coursesRepository)
        {
            _coursesRepository = coursesRepository;
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
    }
}
