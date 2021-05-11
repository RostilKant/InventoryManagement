using System;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.Contracts
{
    public interface IConsumableRepository : IRepositoryBase<Consumable>
    {
        Task<PagedList<Consumable>> GetAllConsumablesAsync(Guid userId, ConsumableParameters consumableParameters);
        Task<Consumable> GetConsumableAsync(Guid userId, Guid id, bool trackChanges = false);
        void UpdateConsumable(Consumable consumable);
        void CreateConsumable(Consumable consumable);
        void DeleteConsumable(Consumable consumable);
    }
}