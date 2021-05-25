using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Entities.DataTransferObjects.License;
using Entities.RequestFeatures;
using InventoryManagement.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Services.Contracts;

namespace InventoryManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> PutLicense(Guid id, LicenseForUpdateDto licenseForUpdate)
        {
            var license = await _licenseService.UpdateAsync(id, licenseForUpdate);

            return license ? NoContent() : NotFound();
        }
        
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
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
