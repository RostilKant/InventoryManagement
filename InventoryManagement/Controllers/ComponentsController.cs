using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Entities.DataTransferObjects.Component;
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
    public class ComponentsController : ControllerBase
    {
        private readonly IComponentService _componentService;
        
        public ComponentsController(IComponentService componentService)
        {
            _componentService = componentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetComponents([FromQuery] ComponentParameters componentParameters)
        {
            var (components, metadata) = await _componentService.GetManyAsync(componentParameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(components);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetComponent(Guid id)
        {
            var component = await _componentService.GetOneById(id);
            
            return component == null ? NotFound() : Ok(component);
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> PutComponent(Guid id, ComponentForUpdateDto componentForUpdate)
        {
            var component = await _componentService.UpdateAsync(id, componentForUpdate);

            return component ? NoContent() : NotFound();
        }
        
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> PostComponent(ComponentForCreationDto componentForCreation)
        {
            var component = await _componentService.CreateAsync(componentForCreation);

            return CreatedAtAction("GetComponent", new { id = component.Id }, component);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteComponent(Guid id)
        {
            var component = await _componentService.DeleteAsync(id);

            return component ? NoContent() : NotFound();
        }
    }
}
