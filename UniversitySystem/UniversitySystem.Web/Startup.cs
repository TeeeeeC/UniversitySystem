using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniversitySystem.Business;
using UniversitySystem.Business.Interfaces;
using UniversitySystem.Business.IoC;
using UniversitySystem.DataAccess.IoC;

namespace UniversitySystem.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataAccess(Configuration);
            services.AddBusiness(Configuration);
            services.AddScoped(typeof(IStudentService), typeof(StudentService));
            services.AddScoped(typeof(ICourseService), typeof(CourseService));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddHttpContextAccessor();
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error/errorLocal");
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Default",
                    pattern: "/{controller}/{action}/{id?}",
                    defaults: new { controller = "student", action = "login" });
            });
        }
    }
}
