using System.ComponentModel.DataAnnotations;
using UniversitySystem.Web.Attributes;

namespace UniversitySystem.Web.Models.Student
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter email")]
        [CustomEmailAddress(ErrorMessage = "Please enter valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
