﻿using System;
using System.ComponentModel.DataAnnotations;
using Entities.Enums;

namespace Entities.DataTransferObjects.Component
{
    public abstract class ComponentForManipulationDto
    {
        [Required(ErrorMessage = "Component name is required field")]
        [MaxLength(100, ErrorMessage = "Maximum length of name is 100 characters")]
        public string Name { get; set; }

        [Required]
        public string Serial { get; set; }
        
        [Required]
        public ComponentCategory Category { get; set; }
        
        [Required]
        public AssetStatus Status { get; set; }
        
        [Required(ErrorMessage = "Manufacturer is required field")]
        public string Manufacturer { get; set; }
        
        public string OfficeAddress { get; set; }
        
        [Required(ErrorMessage = "PurchaseCost is required field")]
        public decimal PurchaseCost { get; set; }
        
        [Required(ErrorMessage = "PurchaseDate is required field")]
        public DateTime PurchaseDate { get; set; }
    }
}