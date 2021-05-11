using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects.User;
using Entities.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
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

        private User _user;

        public AuthenticationService(UserManager<User> userManager, ILogger<AuthenticationService> logger, IMapper mapper,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<bool> RegisterUserAsync(UserForRegistrationDto userForRegistration,
            ModelStateDictionary modelState)
        {
            var user = _mapper.Map<User>(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    modelState.TryAddModelError(error.Code, error.Description);
                }

                return false;
            }

            await _userManager.AddToRoleAsync(user, userForRegistration.Role ?? "User");
            return true;
        }

        public async Task<bool> AuthenticateUserAsync(UserForAuthenticationDto userForAuthentication)
        {
            _user = await _userManager.FindByEmailAsync(userForAuthentication.Email);
            return (_user != null && await _userManager.CheckPasswordAsync(_user,
                userForAuthentication.Password));
        }

        public async Task<string> CreateTokenAsync()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
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
                expires: DateTime.Now.AddHours(Convert.ToDouble(jwtSettings.GetSection("expires").Value)),
                signingCredentials: signingCredentials
            );
            return tokenOptions;
        }
    }
}