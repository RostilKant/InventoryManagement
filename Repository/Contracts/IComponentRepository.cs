using System;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.Contracts
{
    public interface IComponentRepository : IRepositoryBase<Component>
    {
        Task<PagedList<Component>> GetAllComponentsAsync(ComponentParameters componentParameters);
        Task<Component> GetComponentAsync(Guid id);
        void UpdateComponent(Component component);
        void CreateComponent(Component component);
        void DeleteComponent(Component component);
    }
}