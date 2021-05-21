using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using UniversitySystem.Web.Models;

namespace UniversitySystem.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult ErrorLocal(
        [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException("This shouldn't be invoked in non-development environments.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return View(new ErrorViewModel { Message = context.Error.Message });
        }

        public IActionResult Error() => Problem();
    }
}
