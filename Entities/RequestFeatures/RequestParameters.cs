using Entities.Enums;

namespace Entities.RequestFeatures
{
    public abstract class RequestParameters
    {
        private const int MaxPageSize = 50;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        
        public string SearchTerm { get; set; }
        public string OrderBy { get; set; }
    }

    public class EmployeeParameters : RequestParameters
    {
        public EmployeeParameters()
        {
            OrderBy = "employmentDate";
        }
        public EmployeeDepartment? Department { get; set; } = null;
        public string Job { get; set; }
        public string OfficeAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }

    public class DeviceParameters : RequestParameters
    {
        public DeviceParameters()
        {
            OrderBy = "purchaseDate";
        }
        public string Model { get; set; }

        public DeviceCategory? Category { get; set; } = null;

        public AssetStatus? Status { get; set; } = null;
        
        public string Manufacturer { get; set; }
        
        public string OfficeAddress { get; set; }
    }
    
    public class AccessoryParameters : RequestParameters
    {
        public AccessoryParameters()
        {
            OrderBy = "purchaseDate";
        }
        
        public string Name { get; set; }

        public AccessoryCategory Category { get; set; }
        
        public AssetStatus Status { get; set; }
        
        public string ModelNumber { get; set; }

        public string Manufacturer { get; set; }
        
        public string OfficeAddress { get; set; }
    }
    
    public class ConsumableParameters : RequestParameters
    {
        public ConsumableParameters()
        {
            OrderBy = "purchaseDate";
        }
   
        public string Name { get; set; }
        
        public ConsumableCategory Category { get; set; }

        public AssetStatus Status { get; set; }
        
        public string Manufacturer { get; set; }
        
        public string OfficeAddress { get; set; }
    }
    
    public class ComponentParameters : RequestParameters
    {
        public ComponentParameters()
        {
            OrderBy = "purchaseDate";
        }
        
        public string Name { get; set; }

        public string Serial { get; set; }
        
        public ComponentCategory Category { get; set; }
        
        public AssetStatus Status { get; set; }
        
        public string Manufacturer { get; set; }
        
        public string OfficeAddress { get; set; }
    }
    
    public class LicenseParameters : RequestParameters
    {
        public LicenseParameters()
        {
            OrderBy = "purchaseDate";
        }
   
        public string Name { get; set; }
        
        public LicenseCategory Category { get; set; }
        
        public string ProductKey { get; set; }
        
        public string LicensedToEmail { get; set; }
        
        public string Manufacturer { get; set; }
        
        public bool IsReAssignable { get; set; }
    }
}