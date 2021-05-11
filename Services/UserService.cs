using System;
using System.Threading.Tasks;
using Entities.IdentityModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Services.Contracts;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<User> userManager, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<T> BindAssetWithUserAsync<T>(Guid userId, T entity)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            
            if (user == null)
                _logger.LogWarning("User with such id {Id} wasn't found", userId);

            typeof(T).GetProperty("User")?.SetValue(entity, user);
            
            return entity;
        }
    }
}