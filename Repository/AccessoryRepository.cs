using System;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Repository.Extensions;

namespace Repository
{
    public class AccessoryRepository : RepositoryBase<Accessory>, IAccessoryRepository
    {
        public AccessoryRepository(ApplicationContext applicationContext) 
            : base(applicationContext)
        {
        }

        public async Task<PagedList<Accessory>> GetAllAccessoriesAsync(Guid userId,
            AccessoryParameters accessoryParameters)
        {
            var result = await FindByCondition(x => x.User.Id.Equals(userId))
                .FilterBy(accessoryParameters)
                .Search(accessoryParameters.SearchTerm)
                .Sort(accessoryParameters.OrderBy)
                .Include(x => x.Device)
                .ToListAsync();
            
            return PagedList<Accessory>.ToPagedList(result, accessoryParameters.PageNumber, accessoryParameters.PageSize);
        }

        public async Task<Accessory> GetAccessoryAsync(Guid userId, Guid id, bool trackChanges = false) => 
            await FindByCondition(x => x.Id.Equals(id) && x.User.Id.Equals(userId), trackChanges)
                .SingleOrDefaultAsync();

        public void UpdateAccessory(Accessory accessory) => Update(accessory);

        public void CreateAccessory(Accessory accessory) => Create(accessory);

        public void DeleteAccessory(Accessory accessory) => Delete(accessory);
    }
}