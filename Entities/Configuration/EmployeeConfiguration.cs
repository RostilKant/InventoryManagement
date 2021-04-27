using System;
using Entities.Enums;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasData(
                new Employee
                {
                    Id = new Guid("74257e60-490f-472f-ae81-17c9518a7ee6"),
                    FirstName = "Rose",
                    LastName = "Lee",
                    Phone = "+380734098995",
                    Department = EmployeeDepartment.Software_Development,
                    Job = "ASP.NET Core Developer",
                    EmploymentDate = new DateTime(2021, 4, 10),
                    OfficeAddress = "Kaliko Str. 127",
                    Address = "Big Guy Str.",
                    City = "New-Popone",
                    State = "Jorji",
                    Country = "New USexico",
                    ZipCode = "129823" 
                }, 
                new Employee
                {
                    Id = new Guid("4c3c2c62-0ee8-4abb-93fa-d9bbf185d1d2"),
                    FirstName = "Kent",
                    LastName = "Zet",
                    Phone = "+380734098996",
                    Department = EmployeeDepartment.Software_Development,
                    Job = "Unity Developer",
                    EmploymentDate = new DateTime(2021, 5, 22),
                    OfficeAddress = "Kaliko Str. 127",
                    Address = "Sunnie Star Str.",
                    City = "New-Carbon",
                    State = "Hemprane",
                    Country = "New USexico",
                    ZipCode = "112522" 
                },  new Employee
                {
                    Id = new Guid("e5efeedd-207b-422c-8637-b2a4564d0737"),
                    FirstName = "Nikkie",
                    LastName = "Lol",
                    Phone = "+380734098997",
                    Department = EmployeeDepartment.Hardware_Development,
                    Job = "Drones Developer",
                    EmploymentDate = new DateTime(2021, 5, 1),
                    OfficeAddress = "Kaliko Str. 127",
                    Address = "Sad dick Str.",
                    City = "New-Cockie",
                    State = "Mranda",
                    Country = "New USexico",
                    ZipCode = "542861" 
                }
            );
        }
    }
}