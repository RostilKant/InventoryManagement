using System;
using System.Threading.Tasks;
using Entities.Models;

namespace Services.Contracts
{
    public interface IUserService
    {
        public Task<T> BindAssetWithUserAsync<T>(Guid userId, T entity);
    }
}