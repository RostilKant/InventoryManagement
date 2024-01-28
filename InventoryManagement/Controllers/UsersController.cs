using System;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Entities.DataTransferObjects.User;
using Entities.Settings;
using InventoryManagement.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        public async Task<IActionResult> RegisterOrganization([FromBody] OrganizationForRegistrationDto organizationForRegistration,
            [FromServices] IConfiguration config)
        {
            var (isCreated, errors) = await _authenticationService.RegisterOrganizationAsync(organizationForRegistration);

            if (isCreated)
            {
                var root = (IConfigurationRoot)config;
                root.Reload();
                
                return StatusCode(201);
            }

            foreach (var error in errors) ModelState.AddModelError(error.Code, error.Description);    
            
            return BadRequest(ModelState);
        }
        
        [HttpPost("organization/migrate")]
        [Authorize]
        public async Task<IActionResult> MigrateData([FromQuery] bool isSeparate, [FromQuery] string tenantId,
            [FromServices] ApplicationContext context, [FromServices] TenantConfigurationDbContext tenantConfigurationDbContext,
            [FromServices] IConfiguration configuration)
        {
            var tenantSettings = configuration.GetSection("TenantSettings").Get<TenantSettings>();

            var tenant = tenantConfigurationDbContext.Tenants
                .FirstOrDefault(x => x.Id == tenantId);

            if (!isSeparate)
            {
                var accessories = await context.Accessories.ToListAsync();

                var aspNetUsers = await context.Users.ToListAsync();

                var newConnectionString = tenantSettings.Defaults.ConnectionString
                    .Replace("counter", tenantSettings.Defaults.Counter.ToString());
                context.Database.SetConnectionString(newConnectionString);

                //migrate data

                context.Accessories.AddRange(accessories);
                context.Users.AddRange(aspNetUsers);
                await context.SaveChangesAsync();

                //delete old database

                context.Database.SetConnectionString(tenant.ConnectionString);
                await context.Database.EnsureDeletedAsync();

                tenant.ConnectionString = newConnectionString;
                tenant.IsSeparate = false;
                await tenantConfigurationDbContext.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration,
            [FromQuery] string tenantId, [FromServices] ApplicationContext _context,
            [FromServices] TenantConfigurationDbContext _tenantConfigurationDbContext)
        {
            var tenants = await _tenantConfigurationDbContext.Tenants
                .FirstOrDefaultAsync(x => x.Id == tenantId);

            _context.Database.SetConnectionString(tenants.ConnectionString);

            try
            {
                if (_context.Database.GetMigrations().Any()) await _context.Database.MigrateAsync();
            }
            catch
            {
                // ignored
            }

            var (isCreated, errors) = await _authenticationService.RegisterUserAsync(userForRegistration);

            if (isCreated) return StatusCode(201);

            foreach (var error in errors) ModelState.AddModelError(error.Code, error.Description);    
            
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user, [FromQuery] string tenantId)
        {
            if (!await _authenticationService.AuthenticateUserAsync(tenantId, user))
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