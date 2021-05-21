using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
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
        private readonly IToastNotification _toastNotification;

        public CourseController(ICourseService courseService,
            IToastNotification toastNotification)
        {
            _courseService = courseService;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            return View(await this.GetManageCourseViewModel(page));
        }

        public async Task<IActionResult> MyCourses(int page = 1)
        {
            var response = await _courseService.GetAllByStudentIdAsync(page, 5);
            var courses = response.Courses
                .Select(c => new CourseViewModel { CourseId = c.CourseId, Name = c.Name })
                .ToList();

            return View(new ManageCourseViewModel { CurrentPage = page, TotalPages = response.TotalPages, Courses = courses });
        }

        [HttpGet]
        public async Task<IActionResult> Join(int id, int page)
        {
            var isSuccessFull = await _courseService.JoinAsync(id);
            if (isSuccessFull)
            {
                _toastNotification.AddSuccessToastMessage("You are joined to this course.");
                return RedirectToAction("MyCourses");
            }

            _toastNotification.AddErrorToastMessage("You are already registered to this course.");

            return RedirectToAction("Index", new { page });
        }

        [HttpGet]
        public async Task<IActionResult> ManageCourses(int page = 1)
        {
            return View(await this.GetManageCourseViewModel(page));
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

                _toastNotification.AddSuccessToastMessage("New course was created.");

                return RedirectToAction("ManageCourses", new { page });
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

                _toastNotification.AddSuccessToastMessage("The course was updated.");

                return RedirectToAction("ManageCourses", new { page });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, int page)
        {
            await _courseService.DeleteAsync(id);

            _toastNotification.AddSuccessToastMessage("The course was deleted.");

            return RedirectToAction("ManageCourses", new { page });
        }

        private async Task<ManageCourseViewModel> GetManageCourseViewModel(int page)
        {
            var response = await _courseService.GetAsync(page, 5);
            var courses = response.Courses
                .Select(c => new CourseViewModel { CourseId = c.CourseId, Name = c.Name })
                .ToList();

            return new ManageCourseViewModel { CurrentPage = page, TotalPages = response.TotalPages, Courses = courses };
        }
    }
}
