using System;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.Contracts
{
    public interface IAccessoryRepository : IRepositoryBase<Accessory>
    {
        Task<PagedList<Accessory>> GetAllAccessoriesAsync(Guid userId, AccessoryParameters accessoryParameters);
        Task<Accessory> GetAccessoryAsync(Guid userId, Guid id, bool trackChanges = false);
        void UpdateAccessory(Accessory accessory);
        void CreateAccessory(Accessory accessory);
        void DeleteAccessory(Accessory accessory);
    }
}