using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.DataTransferObjects.License;
using Entities.RequestFeatures;

namespace Services.Contracts
{
    public interface ILicenseService
    {
        public Task<(IEnumerable<LicenseDto>, Metadata)> GetManyAsync(LicenseParameters licenseParameters);
        public Task<LicenseDto> GetOneById(Guid id);
        public Task<LicenseDto> CreateAsync(LicenseForCreationDto licenseForCreation);
        public Task<bool> DeleteAsync(Guid id);
        public Task<bool> UpdateAsync(Guid id, LicenseForUpdateDto licenseForUpdate);
    }
}