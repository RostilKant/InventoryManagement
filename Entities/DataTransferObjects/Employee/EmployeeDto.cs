using System;
using System.Collections.Generic;
using Entities.DataTransferObjects.Device;
using Entities.DataTransferObjects.License;
using Entities.Enums;

namespace Entities.DataTransferObjects.Employee
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        
        public string Job { get; set; }

        public string Phone { get; set; }
        
        public EmployeeDepartment Department { get; set; }
        
        public string OfficeAddress { get; set; }

        public DateTime EmploymentDate { get; set; }
        
        public string Address { get; set; }
        
        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }
        
        public string ZipCode { get; set; }

        public ICollection<DeviceDto> Devices { get; set; }
        public ICollection<LicenseDto> Licenses { get; set; }
    }
}