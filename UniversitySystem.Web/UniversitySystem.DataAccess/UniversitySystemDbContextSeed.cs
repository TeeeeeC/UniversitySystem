using System.Collections.Generic;
using System.Linq;
using UniversitySystem.DataAccess.Models;

namespace UniversitySystem.DataAccess
{
    public static class UniversitySystemDbContextSeed
    {
        public static void Seed(UniversitySystemDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Students.Any())
            {
                return;
            }

            var students = new List<Student>()
            { 
                new Student { Email = "john@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123456") },
                new Student { Email = "steve@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("1234567") },
                new Student { Email = "lily@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("12345678") },
            };
            context.Students.AddRange(students);
            context.SaveChanges();

            var courses = new List<Course>()
            {
                new Course { Name = "C# Basics" },
                new Course { Name = "C# Advance" },
                new Course { Name = "Databases" },
            };
            context.Courses.AddRange(courses);
            context.SaveChanges();

            var student = context.Students.FirstOrDefault();
            var course = context.Courses.FirstOrDefault();
            if (student != null && course != null)
            {
                context.StudentCourses.Add(new StudentCourse
                {
                    CourseID = course.CourseId,
                    StudentID = student.StudentId
                });
            }
            context.SaveChanges();
        }
    }
}
