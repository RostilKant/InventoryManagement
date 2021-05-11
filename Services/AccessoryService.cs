using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects.Accessory;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Repository.Contracts;
using Services.Contracts;
using Services.ServiceExtensions;

namespace Services
{
    public class AccessoryService : IAccessoryService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILogger<AccessoryService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserService _userService;
        
        private Guid CurrentUserId =>  _contextAccessor.HttpContext?.User.GetCurrentUserId() ?? Guid.Empty;

        public AccessoryService(IRepositoryManager repositoryManager, ILogger<AccessoryService> logger, IMapper mapper, IHttpContextAccessor contextAccessor, IUserService userService)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _userService = userService;
        }

        public async Task<(IEnumerable<AccessoryDto>, Metadata)> GetManyAsync(AccessoryParameters accessoryParameters)
        {
            var accessories = await _repositoryManager.Accessory.GetAllAccessoriesAsync(CurrentUserId, accessoryParameters);
            
            if (accessories == null)
                _logger.LogWarning("There are no accessories in db!");

            return (_mapper.Map<IEnumerable<AccessoryDto>>(accessories), accessories?.Metadata);
        }

        public async Task<AccessoryDto> GetOneById(Guid id)
        {
            var accessory = await _repositoryManager.Accessory.GetAccessoryAsync(CurrentUserId, id);

            if (accessory == null)
                _logger.LogWarning("Accessory with id {Id} doesn't exists in db!", id);

            return _mapper.Map<AccessoryDto>(accessory);
        }

        public async Task<AccessoryDto> CreateAsync(AccessoryForCreationDto accessoryForCreation)
        {
            var accessory = _mapper.Map<Accessory>(accessoryForCreation);
            accessory = await _userService.BindAssetWithUserAsync(CurrentUserId, accessory);
            
            _repositoryManager.Accessory.CreateAccessory(accessory);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<AccessoryDto>(accessory);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var accessory = await _repositoryManager.Accessory.GetAccessoryAsync(CurrentUserId, id);

            if (accessory == null)
            {
                _logger.LogWarning("Accessory with id {Id} doesn't exists in db!", id);
                return false;
            }
            
            _repositoryManager.Accessory.DeleteAccessory(accessory);
            await _repositoryManager.SaveAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(Guid id, AccessoryForUpdateDto accessoryForUpdate)
        {
            var accessory = await _repositoryManager.Accessory.GetAccessoryAsync(CurrentUserId, id, true);

            if (accessory == null)
            {
                _logger.LogWarning("Accessory with id {Id} doesn't exists in db!", id);
                return false;
            }

            _mapper.Map(accessoryForUpdate, accessory);
            
            _repositoryManager.Accessory.UpdateAccessory(accessory);
            await _repositoryManager.SaveAsync();

            return true;
        }
    }
}