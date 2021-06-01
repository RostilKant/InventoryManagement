using System.Threading.Tasks;
using Entities.DataTransferObjects.User;
using InventoryManagement.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace InventoryManagement.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public UsersController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
            =>
                await _authenticationService.RegisterUserAsync(userForRegistration, ModelState) ?
                    StatusCode(201) : BadRequest(ModelState);
        
        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _authenticationService.AuthenticateUserAsync(user))
                return Unauthorized();
            
            var (token, exp) = await _authenticationService.CreateTokenAsync();
            return Ok(new {token, exp });
        }
    }
}