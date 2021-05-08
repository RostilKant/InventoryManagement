using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Entities.DataTransferObjects.Device;
using Entities.DataTransferObjects.Employee;
using Entities.Enums;
using Entities.RequestFeatures;
using InventoryManagement.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Contracts;

namespace InventoryManagement.Controllers
{
    [Route("api/employees")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
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
        [Consumes(MediaTypeNames.Application.Json)]
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
        
        [HttpPost("{id:guid}/devices/assign")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AssignEmployeeProject(Guid id, [FromBody] DeviceForAssignDto deviceForAssignDto)
        {
            deviceForAssignDto.AssignType = AssetAssignType.Adding;
            var result = await _employeeService.AssignDeviceAsync(id, deviceForAssignDto.Id);
            
            return result ? NoContent() : NotFound();
        }
        
        [HttpPost("{id:guid}/devices/unassign")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UnAssignEmployeeProject(Guid id, [FromBody] DeviceForAssignDto deviceForAssignDto)
        {
            deviceForAssignDto.AssignType = AssetAssignType.Removing;
            var result = await _employeeService.UnAssignDeviceAsync(id, deviceForAssignDto.Id);
            
            return result ? NoContent() : NotFound();
        }
        
    }
}