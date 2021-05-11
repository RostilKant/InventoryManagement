using System;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.Contracts
{
    public interface ILicenseRepository : IRepositoryBase<License>
    {
        Task<PagedList<License>> GetAllLicensesAsync(Guid userId, LicenseParameters licenseParameters);
        Task<License> GetLicenseAsync(Guid userId, Guid id, bool trackChanges = false);
        void UpdateLicense(License license);
        void CreateLicense(License license);
        void DeleteLicense(License license);
    }
}