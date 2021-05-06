using System;
using Entities.Enums;

namespace Entities.DataTransferObjects.License
{
    public class LicenseForAssignDto
    {
        public Guid Id { get; set; }
        public AssetAssignType AssignType { get; set; }
    }
}