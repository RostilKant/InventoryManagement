using System;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository
{
    public class LicenseRepository : RepositoryBase<License>, ILicenseRepository
    {
        public LicenseRepository(ApplicationContext applicationContext) 
            : base(applicationContext)
        {
        }

        public async Task<PagedList<License>> GetAllLicensesAsync(LicenseParameters licenseParameters)
        {
            var result = await FindAll()
                .Include(x => x.Employees)
                .ToListAsync();
            
            return PagedList<License>.ToPagedList(result, licenseParameters.PageNumber, licenseParameters.PageSize);
        }

        public async Task<License> GetLicenseAsync(Guid id, bool trackChanges = false) =>
            await FindByCondition(x => x.Id.Equals(id), trackChanges)
                .SingleOrDefaultAsync();

        public void UpdateLicense(License license) => Update(license);

        public void CreateLicense(License license) => Create(license);

        public void DeleteLicense(License license) => Delete(license);
    }
}