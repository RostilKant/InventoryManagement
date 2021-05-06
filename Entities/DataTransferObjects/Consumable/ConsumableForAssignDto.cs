using System;
using Entities.Enums;

namespace Entities.DataTransferObjects.Consumable
{
    public class ConsumableForAssignDto
    {
        public Guid Id { get; set; }
        public AssetAssignType AssignType { get; set; }
    }
}