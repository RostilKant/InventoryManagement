using System;
using Entities.Enums;

namespace Entities.DataTransferObjects.Component
{
    public class ComponentForAssignDto
    {
        public Guid Id { get; set; }
        public AssetAssignType AssignType { get; set; }
    }
}