using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects.User;
using Entities.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Services.Contracts;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, ILogger<UserService> logger, IMapper mapper)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<T> BindAssetWithUserAsync<T>(Guid userId, T entity)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            
            if (user == null)
                _logger.LogWarning("User with such id {Id} wasn't found", userId);

            typeof(T).GetProperty("User")?.SetValue(entity, user);
            
            return entity;
        }

        public async Task<(bool, IEnumerable<IdentityError>)> UpdateUserInfoAsync(Guid? userId, UserForUpdateDto userForUpdate)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                _logger.LogWarning("User with such id {Id} wasn't found", userId);
                return (false, new List<IdentityError>
                {
                    new()
                    {
                        Code = "404",
                        Description = "User with such id wasn't found"
                    }
                });
            }

            _mapper.Map(userForUpdate, user);
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded ? (true, null) : (false, result.Errors);
        }

        public async Task<(bool, IEnumerable<IdentityError>)> ChangeUserPasswordAsync(Guid? userId, UserChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            var result = await _userManager.ChangePasswordAsync(user, 
                changePasswordDto.OldPassword, changePasswordDto.NewPassword);
            
            return result.Succeeded ? (true, null) : (false, result.Errors);
        }
        
        public async Task<User> GetCurrentUser(Guid? id) =>
            await _userManager.FindByIdAsync(id.ToString());
    }
}