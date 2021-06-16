using System;
using System.Threading.Tasks;
using Entities.DataTransferObjects.User;
using InventoryManagement.ActionFilters;
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
        
        [HttpPost("update")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateUser([FromBody] UserForUpdateDto userForUpdate)
        {
            var user = await _userService.UpdateUserInfoAsync(User.GetCurrentUserId(), userForUpdate, ModelState);
            return user ? NoContent() : BadRequest(ModelState);
        }
        
        [HttpPost("change-pass")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> ChangeUserPassword([FromBody] UserChangePasswordDto changePasswordDto)
        {
            var user = await _userService.ChangeUserPasswordAsync(User.GetCurrentUserId(), changePasswordDto, ModelState);
            return user ? NoContent() : BadRequest(ModelState);
        }
    }
}