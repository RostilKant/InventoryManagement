﻿using System;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.Contracts
{
    public interface IComponentRepository : IRepositoryBase<Component>
    {
        Task<PagedList<Component>> GetAllComponentsAsync(Guid userId, ComponentParameters componentParameters);
        Task<Component> GetComponentAsync(Guid userId, Guid id, bool trackChanges = false);
        void UpdateComponent(Component component);
        void CreateComponent(Component component);
        void DeleteComponent(Component component);
    }
}