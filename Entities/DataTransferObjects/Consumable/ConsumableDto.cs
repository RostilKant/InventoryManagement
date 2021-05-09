using System;
using System.ComponentModel.DataAnnotations;
using Entities.DataTransferObjects.Device;
using Entities.Enums;

namespace Entities.DataTransferObjects.Consumable
{
    public class ConsumableDto
    {
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Consumable name is required field")]
        [MaxLength(100, ErrorMessage = "Maximum length of name is 100 characters")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Category is required field")]
        public ConsumableCategory Category { get; set; }
        
        [Required(ErrorMessage = "Status is required field")]
        public AssetStatus Status { get; set; }
        
        [Required(ErrorMessage = "Manufacturer is required field")]
        public string Manufacturer { get; set; }
        
        [Required(ErrorMessage = "PurchaseCost is required field")]
        public decimal PurchaseCost { get; set; }
        
        [Required(ErrorMessage = "PurchaseDate is required field")]
        public DateTime PurchaseDate { get; set; }
        
        public string OfficeAddress { get; set; }
        
        public DeviceDto Device { get; set; }
    }
}