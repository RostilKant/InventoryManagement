using System;
using System.Collections.Generic;
using Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace Entities.IdentityModels
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        
        public ICollection<Employee> Employees { get; set; }
        public ICollection<License> Licenses { get; set; }
        public ICollection<Device> Devices { get; set; }
        public ICollection<Component> Components { get; set; }
        public ICollection<Consumable> Consumables { get; set; }
        public ICollection<Accessory> Accessories { get; set; }
    }
}