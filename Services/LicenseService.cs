using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects.License;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.Extensions.Logging;
using Repository.Contracts;
using Services.Contracts;

namespace Services
{
    public class LicenseService : ILicenseService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILogger<LicenseService> _logger;
        private readonly IMapper _mapper;

        public LicenseService(IRepositoryManager repositoryManager, ILogger<LicenseService> logger, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<LicenseDto>, Metadata)> GetManyAsync(LicenseParameters licenseParameters)
        {
            var licenses = await _repositoryManager.License.GetAllLicensesAsync(licenseParameters);
            
            if (licenses == null)
                _logger.LogWarning("There are no licenses in db!");

            return (_mapper.Map<IEnumerable<LicenseDto>>(licenses), licenses?.Metadata);
        }

        public async Task<LicenseDto> GetOneById(Guid id)
        {
            var license = await _repositoryManager.License.GetLicenseAsync(id);

            if (license == null)
                _logger.LogWarning("There is no license with {Id}", id);

            return _mapper.Map<LicenseDto>(license);
        }

        public async Task<LicenseDto> CreateAsync(LicenseForCreationDto licenseForCreation)
        {
            var license = _mapper.Map<License>(licenseForCreation); 
            
            _repositoryManager.License.CreateLicense(license);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<LicenseDto>(license);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var license = await _repositoryManager.License.GetLicenseAsync(id);
            
            if (license == null)
            {
                _logger.LogWarning("There is no license with {Id}", id);
                return false;
            }
            
            _repositoryManager.License.DeleteLicense(license);
            await _repositoryManager.SaveAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(Guid id, LicenseForUpdateDto licenseForUpdate)
        {
            var license = await _repositoryManager.License.GetLicenseAsync(id, true);

            if (license == null)
            {
                _logger.LogWarning("There is no license with {Id}", id);
                return false;
            }

            _mapper.Map(licenseForUpdate, license);
            _repositoryManager.License.UpdateLicense(license);
            await _repositoryManager.SaveAsync();

            return true;
        }
    }
}