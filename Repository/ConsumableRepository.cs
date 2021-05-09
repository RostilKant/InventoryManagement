using System;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository
{
    public class ConsumableRepository : RepositoryBase<Consumable>, IConsumableRepository
    {
        public ConsumableRepository(ApplicationContext applicationContext) 
            : base(applicationContext)
        {
        }

        public async Task<PagedList<Consumable>> GetAllConsumablesAsync(ConsumableParameters consumableParameters)
        {
            var result = await FindAll()
                .Include(x => x.Device)
                .ToListAsync();

            return PagedList<Consumable>.ToPagedList(result, consumableParameters.PageNumber,
                consumableParameters.PageSize);
        }

        public async Task<Consumable> GetConsumableAsync(Guid id, bool trackChanges = false) =>
            await FindByCondition(x => x.Id.Equals(id), trackChanges)
                .SingleOrDefaultAsync();


        public void UpdateConsumable(Consumable consumable) => Update(consumable);

        public void CreateConsumable(Consumable consumable) => Create(consumable);

        public void DeleteConsumable(Consumable consumable) => Delete(consumable);
    }
}