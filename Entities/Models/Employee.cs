using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Enums;
using Entities.IdentityModels;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models
{
    [Index(nameof(FirstName), nameof(LastName), IsUnique = true)]
    public record Employee : IMustHaveTenant
    {
        [Column("EmployeeId")]
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "First name is required field")]
        [MaxLength(40, ErrorMessage = "Maximum length of name is 40 characters")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Last name is required field")]
        [MaxLength(40, ErrorMessage = "Maximum length of name is 40 characters")]
        public string LastName { get; set; }
        
        [Required]
        public string Phone { get; set; }
        
        [Required(ErrorMessage = "Department is required field")]
        public EmployeeDepartment Department { get; set; }
        
        public string OfficeAddress { get; set; }

        [Required(ErrorMessage = "Job is required field")]
        [MaxLength(30, ErrorMessage = "Maximum length of job title is 30 characters")]
        public string Job { get; set; }
        
        [Required(ErrorMessage = "Employment date is required field")]
        public DateTime EmploymentDate { get; set; }
        
        [Required(ErrorMessage = "Address is required field")]
        public string Address { get; set; }
        
        [Required(ErrorMessage = "City is required field")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required field")]
        public string State { get; set; }

        [Required(ErrorMessage = "Country is required field")]
        public string Country { get; set; }
        
        [Required(ErrorMessage = "ZipCode is required field")]
        public string ZipCode { get; set; }
        
        public ICollection<Device> Devices { get; set; }
        public ICollection<License> Licenses { get; set; }
        
        
        public User User { get; set; }
        
        
        public string TenantId { get; set; }
    }
}