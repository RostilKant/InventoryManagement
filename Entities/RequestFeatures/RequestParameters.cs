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
    }

    public class EmployeeParameters : RequestParameters
    {
        public EmployeeDepartment? Department { get; set; } = null;
        public string Job { get; set; }
        public string OfficeAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }

    public class DeviceParameters : RequestParameters
    {
        public string Model { get; set; }

        public DeviceCategory? Category { get; set; } = null;

        public AssetStatus? Status { get; set; } = null;
        
        public string Manufacturer { get; set; }
        
        public string OfficeAddress { get; set; }
    }
}