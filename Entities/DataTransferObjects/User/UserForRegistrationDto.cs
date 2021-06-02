using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects.User
{
    
    public class UserForRegistrationDto
    {
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string UserName {get;set;}
        [Required] public string CompanyName { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
    }
}