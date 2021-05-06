using System;
using Entities.Enums;

namespace Entities.DataTransferObjects.Accessory
{
    public class AccessoryForAssignDto
    {
        public Guid Id { get; set; }
        public AssetAssignType AssignType { get; set; }
    }
}