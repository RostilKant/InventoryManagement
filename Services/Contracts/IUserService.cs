using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.DataTransferObjects.User;
using Entities.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace Services.Contracts
{
    public interface IUserService
    {
        public Task<T> BindAssetWithUserAsync<T>(Guid userId, T entity);

        public Task<(bool, IEnumerable<IdentityError>)> UpdateUserInfoAsync(Guid? userId, UserForUpdateDto userForUpdate);

        public Task<(bool, IEnumerable<IdentityError>)> ChangeUserPasswordAsync(Guid? userId, UserChangePasswordDto changePasswordDto);

        public Task<User> GetCurrentUser(Guid? id);
    }
}