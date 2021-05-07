﻿using System;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository
{
    public class AccessoryRepository : RepositoryBase<Accessory>, IAccessoryRepository
    {
        public AccessoryRepository(ApplicationContext applicationContext) 
            : base(applicationContext)
        {
        }

        public async Task<PagedList<Accessory>> GetAllAccessoriesAsync(AccessoryParameters accessoryParameters)
        {
            var result = await FindAll()
                .ToListAsync();
            
            return PagedList<Accessory>.ToPagedList(result, accessoryParameters.PageNumber, accessoryParameters.PageSize);
        }

        public async Task<Accessory> GetAccessoryAsync(Guid id) => 
            await FindByCondition(x => x.Id.Equals(id))
                .SingleOrDefaultAsync();


        public void UpdateAccessory(Accessory accessory) => Update(accessory);

        public void CreateAccessory(Accessory accessory) => Create(accessory);

        public void DeleteAccessory(Accessory accessory) => Delete(accessory);
    }
}