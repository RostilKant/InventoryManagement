using System;
using Entities.Enums;

namespace Entities.DataTransferObjects.Device
{
    public class DeviceForAssignDto
    {
        public Guid Id { get; set; }
        public AssetAssignType AssignType { get; set; }
    }
}