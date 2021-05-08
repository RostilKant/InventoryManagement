using System;
using System.Threading.Tasks;
using Entities.DataTransferObjects.Device;
using Entities.RequestFeatures;
using InventoryManagement.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Contracts;

namespace InventoryManagement.Controllers
{
    [Route("api/devices")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DevicesController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

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
        public async Task<IActionResult> PostProject([FromBody] DeviceForCreationDto projectForCreation)
        {
            var projectDto = await _deviceService.CreateAsync(projectForCreation);
            return CreatedAtAction("GetDevices", new {id = projectDto.Id}, projectDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var project = await _deviceService.DeleteAsync(id);
            return project ? NoContent() : NotFound();
        }
        
        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] DeviceForUpdateDto projectForUpdate)
        {
            var project = await _deviceService.UpdateAsync(id, projectForUpdate);
            return project ? NoContent() : NotFound();
        }
    }
}