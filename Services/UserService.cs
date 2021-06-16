using System;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects.User;
using Entities.IdentityModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        public async Task<bool> UpdateUserInfoAsync(Guid? userId, UserForUpdateDto userForUpdate, ModelStateDictionary modelState)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                _logger.LogWarning("User with such id {Id} wasn't found", userId);
                return false;
            }

            _mapper.Map(userForUpdate, user);
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return true;
            
            foreach (var error in result.Errors)
                modelState.TryAddModelError(error.Code, error.Description);

            return false;
        }

        public async Task<bool> ChangeUserPasswordAsync(Guid? userId, UserChangePasswordDto changePasswordDto, ModelStateDictionary modelState)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            var result = await _userManager.ChangePasswordAsync(user, 
                changePasswordDto.OldPassword, changePasswordDto.NewPassword);
            
            if (result.Succeeded) return true;
            
            foreach (var error in result.Errors)
                modelState.TryAddModelError(error.Code, error.Description);

            return false;
        }
    }
}