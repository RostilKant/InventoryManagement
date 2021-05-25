using System;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.Device;
using Entities.DataTransferObjects.Employee;
using Entities.Enums;
using Entities.RequestFeatures;
using InventoryManagement.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Services.Contracts;

namespace InventoryManagement.Controllers
{
    [Authorize]
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees([FromQuery] EmployeeParameters employeeParameters)
        {
            var (employees, metadata) = await _employeeService.GetManyAsync(employeeParameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            
            return Ok(employees);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEmployee(Guid id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            return employee == null ? NotFound() : Ok(employee);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> PostEmployee([FromBody] EmployeeForCreationDto employeeForCreation)
        {
            var employeeDto = await _employeeService.CreateAsync(employeeForCreation);
            return CreatedAtAction("GetEmployee", new {id = employeeDto.Id}, employeeDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var employee = await _employeeService.DeleteAsync(id);
            return employee ? NoContent() : NotFound();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> PutEmployee(Guid id, [FromBody] EmployeeForUpdateDto employeeForUpdate)
        {
            var employee = await _employeeService.UpdateAsync(id, employeeForUpdate);
            return employee ? NoContent() : NotFound();
        }

        [HttpGet("{id:guid}/devices")]
        public async Task<IActionResult> GetEmployeeDevices(Guid id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            return Ok(employee.Devices);
        }
        
        [HttpGet("{id:guid}/licenses")]
        public async Task<IActionResult> GetEmployeeLicenses(Guid id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            return Ok(employee.Licenses);
        }

        [HttpPut("{id:guid}/devices/manipulate")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> ManipulateEmployeeDevice(Guid id, [FromQuery] AssetForAssignDto assetForAssign)
            => await _employeeService.ManipulateDeviceAsync(id, assetForAssign) ? NoContent() : NotFound();

        [HttpPut("{id:guid}/licenses/manipulate")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> ManipulateEmployeeLicense(Guid id,
            [FromQuery] AssetForAssignDto assetForAssign)
            => await _employeeService.ManipulateLicenseAsync(id, assetForAssign) ? NoContent() : NotFound();
    }
}