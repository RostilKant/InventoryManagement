using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities.DataTransferObjects.Accessory;
using Entities.DataTransferObjects.Component;
using Entities.DataTransferObjects.Consumable;

namespace Entities.DataTransferObjects.Device
{
    public class DeviceDto
    {
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Device title is required field")]
        [MaxLength(40, ErrorMessage = "Maximum length of title is 40 characters")]
        public string Model { get; set; }
        
        [Required(ErrorMessage = "Serial is required field")]
        [MaxLength(40, ErrorMessage = "Maximum length of Serial is 40 characters")]
        public string Serial { get; set; }

        [Required(ErrorMessage = "Category is required field")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Status is required field")]
        public string Status { get; set; }
        
        [Required(ErrorMessage = "Manufacturer is required field")]
        public string Manufacturer { get; set; }
        
        public string OfficeAddress { get; set; }
        
        [Required(ErrorMessage = "PurchaseCost is required field")]
        public decimal PurchaseCost { get; set; }
        
        [Required(ErrorMessage = "PurchaseDate is required field")]
        public DateTime PurchaseDate { get; set; }
        
        public decimal UpdateCost { get; set; }
        
        public DateTime LastUpdateDate { get; set; }
        
        public string Warranty { get; set; }
        
        public DateTime WarrantyExpires { get; set; }
        
        public string Imei{ get; set; }
        
        public string MacAddress { get; set; }
        
        public string Notes { get; set; }
        
        public ICollection<ComponentDto> Components { get; set; }
        public ICollection<AccessoryDto> Accessories { get; set; }
        public ICollection<ConsumableDto> Consumables { get; set; }
    }
}