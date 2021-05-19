using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UniversitySystem.Business.Interfaces;
using UniversitySystem.Business.Models.Student;
using UniversitySystem.Web.Models.Student;

namespace UniversitySystem.Web.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StudentController(IStudentService studentService,
            IHttpContextAccessor httpContextAccessor)
        {
            _studentService = studentService;
            _httpContextAccessor = httpContextAccessor;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Course");
            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var isSuccess = await _studentService.Login(new LoginRequest { Email = model.Email, Password = model.Password });
            if (isSuccess)
            {
                return RedirectToAction("Index", "Course");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var message = await _studentService.Register(new RegisterRequest { Email = model.Email, Password = model.Password });
            if (string.IsNullOrEmpty(message))
            {
                await _studentService.Login(new LoginRequest { Email = model.Email, Password = model.Password });
                return RedirectToAction("Index", "Course");
            }

            ModelState.AddModelError(string.Empty, message);
            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> Logout()
        {
            await _studentService.Logout();

            return RedirectToAction("Login", "Student");
        }
    }
}
