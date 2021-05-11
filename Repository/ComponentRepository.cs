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
    public class ComponentRepository : RepositoryBase<Component>, IComponentRepository
    {
        public ComponentRepository(ApplicationContext applicationContext) 
            : base(applicationContext)
        {
        }

        public async Task<PagedList<Component>> GetAllComponentsAsync(Guid userId,
            ComponentParameters componentParameters)
        {
            var result = await FindAll()
                .FilterBy(componentParameters)
                .Search(componentParameters.SearchTerm)
                .Sort(componentParameters.OrderBy)
                .Include(x => x.Device)
                .ToListAsync();
            
            return PagedList<Component>.ToPagedList(result, componentParameters.PageNumber, componentParameters.PageSize);
        }

        public async Task<Component> GetComponentAsync(Guid userId, Guid id, bool trackChanges = false) =>
            await FindByCondition(x => x.Id.Equals(id), trackChanges)
                .SingleOrDefaultAsync();


        public void UpdateComponent(Component component) => Update(component);

        public void CreateComponent(Component component) => Create(component);

        public void DeleteComponent(Component component) => Delete(component);
    }
}