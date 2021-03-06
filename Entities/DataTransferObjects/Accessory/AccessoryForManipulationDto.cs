﻿using System;
using System.ComponentModel.DataAnnotations;
using Entities.Enums;

namespace Entities.DataTransferObjects.Accessory
{
    public abstract class AccessoryForManipulationDto
    {
        [Required(ErrorMessage = "Accessory name is required field")]
        [MaxLength(100, ErrorMessage = "Maximum length of name is 100 characters")]
        public string Name { get; set; }

        [Required]
        public AccessoryCategory Category { get; set; }
        
        [Required]
        public AssetStatus Status { get; set; }
        
        [Required(ErrorMessage = "ModelNumber is required field")]
        public string ModelNumber { get; set; }

        [Required(ErrorMessage = "Manufacturer is required field")]
        public string Manufacturer { get; set; }
        
        public string OfficeAddress { get; set; }
        
        [Required(ErrorMessage = "PurchaseCost is required field")]
        public decimal PurchaseCost { get; set; }
        
        [Required(ErrorMessage = "PurchaseDate is required field")]
        public DateTime PurchaseDate { get; set; }
    }
}