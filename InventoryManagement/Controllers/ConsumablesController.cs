using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Entities.DataTransferObjects.Consumable;
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
    public class ConsumablesController : ControllerBase
    {
        private readonly IConsumableService _consumableService;
        
        public ConsumablesController(IConsumableService consumableService)
        {
            _consumableService = consumableService;
        }

        [HttpGet]
        public async Task<IActionResult> GetConsumables(
            [FromQuery] ConsumableParameters consumableParameters)
        {
            var (consumables, metadata) = await _consumableService.GetManyAsync(consumableParameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(consumables);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetConsumable(Guid id)
        {
            var consumable = await _consumableService.GetOneById(id);
            
            return consumable == null ? NotFound() : Ok(consumable);
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> PutConsumable(Guid id, ConsumableForUpdateDto consumableForUpdate)
        {
            var consumable = await _consumableService.UpdateAsync(id, consumableForUpdate);

            return consumable ? NoContent() : NotFound();
        }
        
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> PostConsumable(ConsumableForCreationDto consumableForCreation)
        {
            var consumable = await _consumableService.CreateAsync(consumableForCreation);

            return CreatedAtAction("GetConsumable", new { id = consumable.Id }, consumable);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteConsumable(Guid id)
        {
            var consumable = await _consumableService.DeleteAsync(id);

            return consumable ? NoContent() : NotFound();
        }
    }
}
