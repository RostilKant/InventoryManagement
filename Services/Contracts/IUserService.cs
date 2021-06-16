using System;
using System.Threading.Tasks;
using Entities.DataTransferObjects.User;
using Entities.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Services.Contracts
{
    public interface IUserService
    {
        public Task<T> BindAssetWithUserAsync<T>(Guid userId, T entity);

        public Task<bool> UpdateUserInfoAsync(Guid? userId, UserForUpdateDto userForUpdate, ModelStateDictionary modelState);

        public Task<bool> ChangeUserPasswordAsync(Guid? userId, UserChangePasswordDto changePasswordDto, ModelStateDictionary modelState);
    }
}