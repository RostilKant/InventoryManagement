using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Entities.DataTransferObjects.License;
using Entities.RequestFeatures;
using Newtonsoft.Json;
using Services.Contracts;

namespace InventoryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicensesController : ControllerBase
    {
        private readonly ILicenseService _licenseService;
        
        public LicensesController(ILicenseService licenseService)
        {
            _licenseService = licenseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLicenses([FromQuery] LicenseParameters licenseParameters)
        {
            var (licenses, metadata) = await _licenseService.GetManyAsync(licenseParameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(licenses);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetLicense(Guid id)
        {
            var license = await _licenseService.GetOneById(id);
            
            return license == null ? NotFound() : Ok(license);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutLicense(Guid id, LicenseForUpdateDto licenseForUpdate)
        {
            var license = await _licenseService.UpdateAsync(id, licenseForUpdate);

            return license ? NoContent() : NotFound();
        }
        
        [HttpPost]
        public async Task<IActionResult> PostLicense(LicenseForCreationDto licenseForCreation)
        {
            var license = await _licenseService.CreateAsync(licenseForCreation);

            return CreatedAtAction("GetLicense", new { id = license.Id }, license);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteLicense(Guid id)
        {
            var license = await _licenseService.DeleteAsync(id);

            return license ? NoContent() : NotFound();
        }
    }
}
