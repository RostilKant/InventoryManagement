using System.Threading.Tasks;
using Entities.DataTransferObjects.User;
using InventoryManagement.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.ServiceExtensions;

namespace InventoryManagement.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public UsersController(IAuthenticationService authenticationService, IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }
        
        [HttpPost("organization")]
        public async Task<IActionResult> RegisterOrganization([FromBody] OrganizationForRegistrationDto organizationForRegistration)
        {
            var (isCreated, errors) = await _authenticationService.RegisterOrganizationAsync(organizationForRegistration);

            if (isCreated) return StatusCode(201);

            foreach (var error in errors) ModelState.AddModelError(error.Code, error.Description);    
            
            return BadRequest(ModelState);
        }

        [HttpPost("{tenantId}")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var (isCreated, errors) = await _authenticationService.RegisterUserAsync(userForRegistration);

            if (isCreated) return StatusCode(201);

            foreach (var error in errors) ModelState.AddModelError(error.Code, error.Description);    
            
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _authenticationService.AuthenticateUserAsync(user))
                return Unauthorized();
            
            var (token, exp) = await _authenticationService.CreateTokenAsync();
            return Ok(new {token, exp });
        }
        
        [Authorize]
        [HttpPost("update")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateUser([FromBody] UserForUpdateDto userForUpdate)
        {
            var (user, errors) = await _userService.UpdateUserInfoAsync(User.GetCurrentUserId(), userForUpdate);
            
            if (user) return NoContent();
            
            foreach (var error in errors) ModelState.AddModelError(error.Code, error.Description);
            
            return BadRequest(ModelState);
        }
        
        [Authorize]
        [HttpPost("change-pass")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> ChangeUserPassword([FromBody] UserChangePasswordDto changePasswordDto)
        {
            var (user, errors) = await _userService.ChangeUserPasswordAsync(User.GetCurrentUserId(), changePasswordDto);
            
            if (user) return NoContent();

            foreach (var error in errors) ModelState.AddModelError(error.Code, error.Description);
            
            return BadRequest(ModelState);
        }
        
        [Authorize]
        [HttpGet]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> GetCurrentUser()
         => Ok(await _userService.GetCurrentUser(User.GetCurrentUserId()));
        
    }
}