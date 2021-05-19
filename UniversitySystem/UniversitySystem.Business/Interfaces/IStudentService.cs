using System.Threading.Tasks;
using UniversitySystem.Business.Models.Student;

namespace UniversitySystem.Business.Interfaces
{
    public interface IStudentService
    {
        Task<bool> Login(LoginRequest request);

        Task Logout();

        Task SignIn(StudentSignInRequest request);
    }
}
