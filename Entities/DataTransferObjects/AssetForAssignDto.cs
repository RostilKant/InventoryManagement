using System;
using System.ComponentModel.DataAnnotations;
using Entities.Enums;

namespace Entities.DataTransferObjects
{
    public class AssetForAssignDto
    {
        [Required] public Guid AssetId { get; set; }
        [Required] public AssetAssignType AssignType { get; set; }
    }
}