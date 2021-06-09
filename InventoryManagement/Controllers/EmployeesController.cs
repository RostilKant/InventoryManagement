using System;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.Employee;
using Entities.RequestFeatures;
using InventoryManagement.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> PostEmployee([FromBody] EmployeeForCreationDto employeeForCreationDto)
        {
            var employeeDto = await _employeeService.CreateAsync(employeeForCreationDto);
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
        public async Task<IActionResult> PutEmployee(Guid id, [FromBody] EmployeeForUpdateDto employeeForUpdateDto)
        {
            var employee = await _employeeService.UpdateAsync(id, employeeForUpdateDto);
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
        public async Task<IActionResult> ManipulateEmployeeDevice(Guid id, [FromQuery] AssetForAssignDto assetForAssignDto)
            => await _employeeService.ManipulateDeviceAsync(id, assetForAssignDto) ? NoContent() : NotFound();

        [HttpPut("{id:guid}/licenses/manipulate")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> ManipulateEmployeeLicense(Guid id,
            [FromQuery] AssetForAssignDto assetForAssignDto)
            => await _employeeService.ManipulateLicenseAsync(id, assetForAssignDto) ? NoContent() : NotFound();
    }
}