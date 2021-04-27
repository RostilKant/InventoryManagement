using System;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.Device;
using Entities.DataTransferObjects.Employee;
using Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace InventoryManagement.Controllers
{
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
        public async Task<IActionResult> GetEmployees() =>
            Ok(await _employeeService.GetManyAsync());
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEmployee(Guid id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            return employee == null ? NotFound() : Ok(employee);
        }
        
        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody] EmployeeForCreationDto employeeForCreation)
        {
            if (employeeForCreation == null)
                return BadRequest("EmployeeForCreationDto is null");
            
            if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            
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
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] EmployeeForUpdateDto employeeForUpdate)
        {
            if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            
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
        public async Task<IActionResult> AssignEmployeeProject(Guid id, [FromBody] DeviceForAssignDto projectAssignManipulationDto)
        {
            if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            
            projectAssignManipulationDto.AssignType = AssetAssignType.Adding;
            var result = await _employeeService.AssignDeviceAsync(id, projectAssignManipulationDto.Id);
            
            return result ? NoContent() : NotFound();
        }
        
        [HttpPost("{id:guid}/devices/unassign")]
        public async Task<IActionResult> UnAssignEmployeeProject(Guid id, [FromBody] DeviceForAssignDto projectAssignManipulationDto)
        {
            if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            
            projectAssignManipulationDto.AssignType = AssetAssignType.Removing;
            var result = await _employeeService.UnAssignDeviceAsync(id, projectAssignManipulationDto.Id);
            
            return result ? NoContent() : NotFound();
        }
        
    }
}