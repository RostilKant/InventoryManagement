using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects.Component;
using Entities.DataTransferObjects.Consumable;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.Extensions.Logging;
using Repository.Contracts;
using Services.Contracts;

namespace Services
{
    public class ComponentService : IComponentService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILogger<ComponentService> _logger;
        private readonly IMapper _mapper;

        public ComponentService(IRepositoryManager repositoryManager, ILogger<ComponentService> logger, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<ComponentDto>, Metadata)> GetManyAsync(ComponentParameters componentParameters)
        {
            var components = 
                await _repositoryManager.Component.GetAllComponentsAsync(componentParameters);
            
            if (components == null)
                _logger.LogWarning("There are no components in db!");

            return (_mapper.Map<IEnumerable<ComponentDto>>(components), components.Metadata);
        }

        public async Task<ComponentDto> GetOneById(Guid id)
        {
            var component = await _repositoryManager.Component.GetComponentAsync(id);

            if (component == null)
                _logger.LogWarning("There is no component in db with such id {Id}!", id); 

            return _mapper.Map<ComponentDto>(component);
        }

        public async Task<ComponentDto> CreateAsync(ComponentForCreationDto componentForCreation)
        {
            var component = _mapper.Map<Component>(componentForCreation);

            _repositoryManager.Component.CreateComponent(component);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<ComponentDto>(component);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var component = await _repositoryManager.Component.GetComponentAsync(id);

            if (component == null)
            {
                _logger.LogWarning("There is no component in db with such id {Id}!", id); 
                return false;
            }
            
            _repositoryManager.Component.DeleteComponent(component);
            await _repositoryManager.SaveAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(Guid id, ComponentForUpdateDto componentForUpdate)
        {
            var component = await _repositoryManager.Component.GetComponentAsync(id);

            if (component == null)
            {
                _logger.LogWarning("There is no component in db with such id {Id}!", id); 
                return false;
            }

            _mapper.Map(componentForUpdate, component);
            
            _repositoryManager.Component.UpdateComponent(component);
            await _repositoryManager.SaveAsync();

            return true;
        }
    }
}