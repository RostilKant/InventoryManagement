using System;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.Contracts
{
    public interface IAccessoryRepository : IRepositoryBase<Accessory>
    {
        Task<PagedList<Accessory>> GetAllAccessoriesAsync(AccessoryParameters accessoryParameters);
        Task<Accessory> GetAccessoryAsync(Guid id);
        void UpdateAccessory(Accessory accessory);
        void CreateAccessory(Accessory accessory);
        void DeleteAccessory(Accessory accessory);
    }
}