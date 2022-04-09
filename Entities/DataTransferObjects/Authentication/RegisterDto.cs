using Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects.Authentication
{
    public class RegisterDto
    {
        [Required(ErrorMessage ="User Name is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string PhoneNumer { get; set; }
        public string Password { get; set; }
        public UserTypeEnum UserType { get; set; }
    }
}
