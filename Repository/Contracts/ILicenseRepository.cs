using System;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.Contracts
{
    public interface ILicenseRepository : IRepositoryBase<License>
    {
        Task<PagedList<License>> GetAllComponentsAsync(LicenseParameters licenseParameters);
        Task<License> GetComponentAsync(Guid id);
        void UpdateLicense(License license);
        void CreateLicense(License license);
        void DeleteLicense(License license);
    }
}