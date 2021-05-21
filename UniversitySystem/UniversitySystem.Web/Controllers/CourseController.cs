using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using UniversitySystem.Business.Interfaces;
using UniversitySystem.Business.Models.Course;
using UniversitySystem.Web.Models.Course;

namespace UniversitySystem.Web.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ManageCourses(int page = 1)
        {
            var response = await _courseService.GetAsync(page, 5);
            var courses = response.Courses
                .Select(c => new CourseViewModel { CourseId = c.CourseId, Name = c.Name })
                .ToList();

            return View(new ManageCourseViewModel { CurrentPage = page, TotalPages = response.TotalPages, Courses = courses });
        }

        [HttpGet]
        public IActionResult Create(int page)
        {
            ViewBag.Page = page;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseViewModel model, int page)
        {
            if (this.ModelState.IsValid)
            {
                await _courseService.CreateAsync(new CreateCourseRequest { Name = model.Name });

                return RedirectToAction("ManageCourses", new { page = page });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id, int page)
        {
            var course =  await _courseService.GetByIdAsync(id);
            ViewBag.Page = page;
            return View(new UpdateCourseViewModel { CourseId = id, Name = course.Name });
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCourseViewModel model, int page)
        {
            if (this.ModelState.IsValid)
            {
                await _courseService.UpdateAsync(new UpdateCourseRequest { CourseId = model.CourseId, Name = model.Name });

                return RedirectToAction("ManageCourses", new { page = page });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, int page)
        {
            await _courseService.DeleteAsync(id);

            return RedirectToAction("ManageCourses", new { page = page });
        }
    }
}
