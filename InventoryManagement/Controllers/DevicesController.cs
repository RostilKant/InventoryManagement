using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.Device;
using Entities.Enums;
using Entities.RequestFeatures;
using InventoryManagement.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Contracts;

namespace InventoryManagement.Controllers
{
    [Authorize]
    [Route("api/devices")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DevicesController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        /// <summary>
        /// Get list of all devices
        /// </summary>
        /// <param name="deviceParameters"></param>
        /// <returns>List of devices</returns>
        [HttpGet]
        public async Task<IActionResult> GetDevices([FromQuery] DeviceParameters deviceParameters)
        {
            var (devices, metadata) = await _deviceService.GetManyAsync(deviceParameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(devices);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDevice(Guid id) =>
            Ok(await _deviceService.GetOneById(id));

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> PostDevice([FromBody] DeviceForCreationDto projectForCreation)
        {
            var projectDto = await _deviceService.CreateAsync(projectForCreation);
            return CreatedAtAction("GetDevices", new {id = projectDto.Id}, projectDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDevice(Guid id)
        {
            var project = await _deviceService.DeleteAsync(id);
            return project ? NoContent() : NotFound();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateDevice(Guid id, [FromBody] DeviceForUpdateDto projectForUpdate)
        {
            var result = await _deviceService.UpdateAsync(id, projectForUpdate);
            return result ? NoContent() : NotFound();
        }
        
        [HttpGet("{id:guid}/components")]
        public async Task<IActionResult> GetDeviceComponents(Guid id) =>
            Ok((await _deviceService.GetOneById(id)).Components);
        
        [HttpGet("{id:guid}/consumables")]
        public async Task<IActionResult> GetDeviceConsumables(Guid id) =>
            Ok((await _deviceService.GetOneById(id)).Consumables);
        
        [HttpGet("{id:guid}/accessories")]
        public async Task<IActionResult> GetDeviceAccessories(Guid id) =>
            Ok((await _deviceService.GetOneById(id)).Accessories);

        [HttpPut("{id:guid}/accessories/manipulate")]
        public async Task<IActionResult> ManipulateDeviceAccessory(Guid id,
            [FromQuery] AssetForAssignDto assetForAssign)
            => await _deviceService.ManipulateAccessoryAsync(id, assetForAssign) ? NoContent() : NotFound();


        [HttpPut("{id:guid}/components/manipulate")]
        public async Task<IActionResult> ManipulateDeviceComponent(Guid id,
            [FromQuery] AssetForAssignDto assetForAssign)
            => await _deviceService.ManipulateComponentAsync(id, assetForAssign) ? NoContent() : NotFound();


        [HttpPut("{id:guid}/consumables/manipulate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ManipulateDeviceConsumable(Guid id,
            [FromQuery] AssetForAssignDto assetForAssign)
            => await _deviceService.ManipulateConsumableAsync(id, assetForAssign) ? NoContent() : NotFound();
    }
}