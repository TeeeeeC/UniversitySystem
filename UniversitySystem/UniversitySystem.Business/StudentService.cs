using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniversitySystem.Business.Interfaces;
using UniversitySystem.Business.Models.Student;
using UniversitySystem.DataAccess.Models;
using UniversitySystem.DataAccess.Repositories;

namespace UniversitySystem.Business
{
    public class StudentService : IStudentService
    {
        private readonly IAsyncRepository<Student> _studentsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StudentService(IAsyncRepository<Student> studentsRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _studentsRepository = studentsRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Login(LoginRequest request)
        {
            try
            {
                var studentDb = (await _studentsRepository.List(s => s.Email == request.Email)).FirstOrDefault();
                if (studentDb == null) return false;

                var passwordMatch = BCrypt.Net.BCrypt.Verify(request.Password, studentDb.Password);
                if (passwordMatch)
                {
                    var claims = new List<Claim> { new Claim(ClaimTypes.Email, request.Email) };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties());

                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return false;
        }

        public async Task Logout()
        {
            try
            {
                await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task SignIn(StudentSignInRequest request)
        {
            var studentDb = (await _studentsRepository.List(s => s.Email == request.Email)).FirstOrDefault();
            if (studentDb == null)
            {
                var student = new Student { Email = request.Email, Password = BCrypt.Net.BCrypt.HashPassword(request.Password) };
                await _studentsRepository.Add(student);
            }

            // error
        }
    }
}
