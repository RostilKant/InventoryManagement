using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects.User
{
    public class UserForAuthenticationDto
    {
        [Required(ErrorMessage = "User name is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password name is required")]
        public string Password { get; set; }
    }
}