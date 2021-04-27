using System;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.Device;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetDevices() =>
            Ok(await _deviceService.GetManyAsync());
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDevice(Guid id) =>
            Ok(await _deviceService.GetOneById(id));
        
        [HttpPost]
        public async Task<IActionResult> PostProject([FromBody] DeviceForCreationDto projectForCreation)
        {
            if (projectForCreation == null)
                return BadRequest("ProjectForCreationDto is null");
            
            if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            
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
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] DeviceForUpdateDto projectForUpdate)
        {
            if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            
            var project = await _deviceService.UpdateAsync(id, projectForUpdate);
            return project ? NoContent() : NotFound();
        }
    }
}