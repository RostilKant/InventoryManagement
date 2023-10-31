using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities;
using Entities.DataTransferObjects.User;
using Entities.IdentityModels;
using Entities.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;

namespace Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ApplicationContext _context;

        private User _user;

        public AuthenticationService(UserManager<User> userManager, ILogger<AuthenticationService> logger, IMapper mapper,
            IConfiguration configuration, ApplicationContext context)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
            _context = context;
        }

        public async Task<(bool isCreated, IEnumerable<IdentityError>)> RegisterOrganizationAsync(OrganizationForRegistrationDto organizationForRegistration)
        {
            var newTenant = new Tenant
            {
                Name = organizationForRegistration.Name,
                Id = organizationForRegistration.Name,
                ConnectionString = organizationForRegistration.IsSeparate
                    ? $"Host=localhost;Port=5432;Database=assets-management-{organizationForRegistration.Name};Username=postgres;Password=postgres;"
                    : "Host=localhost;Port=5432;Database=assets-management;Username=postgres;Password=postgres;"
            };
            
            //update appsettings.json
            
            var tenantSettings = _configuration.GetSection("TenantSettings").Get<TenantSettings>();
            
            tenantSettings.Tenants.Add(newTenant);
            
            _configuration.GetSection("TenantSettings").Value = tenantSettings.ToString();
            
            //create database and apply migrations from service collection extensions

            if (organizationForRegistration.IsSeparate)
            {
                _context.Database.SetConnectionString(newTenant.ConnectionString);
                
                if (_context.Database.GetMigrations().Any())
                {
                    await _context.Database.MigrateAsync();
                }
            }

            return (true, null);
        }

        public async Task<(bool, IEnumerable<IdentityError>)> RegisterUserAsync(UserForRegistrationDto userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password);

            if (!result.Succeeded)
            {
                return (false, result.Errors);
            }
            
            await _userManager.AddToRoleAsync(user, userForRegistration.Role ?? "User");
            return (true, null);
        }

        public async Task<bool> AuthenticateUserAsync(UserForAuthenticationDto userForAuthentication)
        {
            _user = await _userManager.FindByEmailAsync(userForAuthentication.Email);
            return (_user != null && await _userManager.CheckPasswordAsync(_user,
                userForAuthentication.Password));
        }

        public async Task<(string, DateTime)> CreateTokenAsync()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return (new JwtSecurityTokenHandler().WriteToken(tokenOptions), tokenOptions.ValidTo);
        }

        private static SigningCredentials GetSigningCredentials()
        {
            var key =
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET")!);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, _user.Id.ToString()),
                new(ClaimTypes.Name, _user.UserName)
            };
            var roles = await _userManager.GetRolesAsync(_user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials
            signingCredentials, IEnumerable<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var tokenOptions = new JwtSecurityToken
            (
                jwtSettings.GetSection("validIssuer").Value,
                jwtSettings.GetSection("validAudience").Value,
                claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToDouble(jwtSettings.GetSection("expires").Value)),
                signingCredentials: signingCredentials
            );
            return tokenOptions;
        }
    }
}