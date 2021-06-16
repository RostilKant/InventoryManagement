using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects.User
{
    public class UserChangePasswordDto
    {
        [Required] public string OldPassword { get; set; }
        [Required] public string NewPassword { get; set; }
    }
}