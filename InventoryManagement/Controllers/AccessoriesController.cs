using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Entities.DataTransferObjects.Accessory;
using Entities.RequestFeatures;
using InventoryManagement.ActionFilters;
using Newtonsoft.Json;
using Services.Contracts;

namespace InventoryManagement.Controllers
{
    [Route("api/accessories")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    public class AccessoriesController : ControllerBase
    {
        private readonly IAccessoryService _accessoryService;

        public AccessoriesController(IAccessoryService accessoryService)
        {
            _accessoryService = accessoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccessories([FromQuery] AccessoryParameters accessoryParameters)
        {
            var (accessories, metadata) = await _accessoryService.GetManyAsync(accessoryParameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(accessories);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAccessory(Guid id)
        {
            var accessory = await _accessoryService.GetOneById(id);
            
            return accessory == null ? NotFound() : Ok(accessory);
        }
        
        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> PutAccessory(Guid id, AccessoryForUpdateDto accessoryForUpdate)
        {
            var accessory = await _accessoryService.UpdateAsync(id, accessoryForUpdate);

            return !accessory ? NotFound() : NoContent();
        }
        
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> PostAccessory(AccessoryForCreationDto accessoryForCreation)
        {
            var accessory = await _accessoryService.CreateAsync(accessoryForCreation);

            return CreatedAtAction("GetAccessory", new { id = accessory.Id }, accessory);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAccessory(Guid id)
        {
            var accessory = await _accessoryService.DeleteAsync(id);
            
            return accessory ? NoContent() : NotFound();
        }
    }
}
