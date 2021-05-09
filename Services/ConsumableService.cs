using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects.Consumable;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.Extensions.Logging;
using Repository.Contracts;
using Services.Contracts;

namespace Services
{
    public class ConsumableService : IConsumableService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILogger<ConsumableService> _logger;
        private readonly IMapper _mapper;

        public ConsumableService(IRepositoryManager repositoryManager, ILogger<ConsumableService> logger,
            IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<ConsumableDto>, Metadata)> GetManyAsync(
            ConsumableParameters consumableParameters)
        {
            var consumables = await _repositoryManager.Consumable.GetAllConsumablesAsync(consumableParameters);

            if (consumables == null)
                _logger.LogWarning("There are no consumables in db!");

            return (_mapper.Map<IEnumerable<ConsumableDto>>(consumables), consumables?.Metadata);
        }

        public async Task<ConsumableDto> GetOneById(Guid id)
        {
            var consumable = await _repositoryManager.Consumable.GetConsumableAsync(id);

            if (consumable == null)
                _logger.LogWarning("There is no consumable in db with such id {Id}!", id);

            return _mapper.Map<ConsumableDto>(consumable);
        }

        public async Task<ConsumableDto> CreateAsync(ConsumableForCreationDto consumableForCreation)
        {
            var consumable = _mapper.Map<Consumable>(consumableForCreation);

            _repositoryManager.Consumable.CreateConsumable(consumable);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<ConsumableDto>(consumable);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var consumable = await _repositoryManager.Consumable.GetConsumableAsync(id);

            if (consumable == null)
            {
                _logger.LogWarning("There is no consumable in db with such id {Id}!", id);
                return false;
            }

            _repositoryManager.Consumable.DeleteConsumable(consumable);
            await _repositoryManager.SaveAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(Guid id, ConsumableForUpdateDto consumableForUpdate)
        {
            var consumable = await _repositoryManager.Consumable.GetConsumableAsync(id, true);

            if (consumable == null)
            {
                _logger.LogWarning("There is no consumable in db with such id {Id}!", id);
                return false;
            }

            _mapper.Map(consumableForUpdate, consumable);
            _repositoryManager.Consumable.UpdateConsumable(consumable);
            await _repositoryManager.SaveAsync();

            return true;
        }
    }
}