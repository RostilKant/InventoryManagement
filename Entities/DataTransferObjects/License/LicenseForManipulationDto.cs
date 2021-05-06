using System;
using System.ComponentModel.DataAnnotations;
using Entities.Enums;

namespace Entities.DataTransferObjects.License
{
    public abstract class LicenseForManipulationDto
    {
        [Required]
        [MaxLength(40, ErrorMessage = "Maximum length of name is 40 characters")]
        public string Name { get; set; }
        
        [Required]
        public LicenseCategory Category { get; set; }
        
        public string ProductKey { get; set; }
        
        public string LicensedToEmail { get; set; }
        
        [Required]
        public DateTime ExpiresAt { get; set; }
        
        [Required(ErrorMessage = "Manufacturer is required field")]
        public string Manufacturer { get; set; }
        
        [Required(ErrorMessage = "PurchaseCost is required field")]
        public decimal PurchaseCost { get; set; }
        
        [Required(ErrorMessage = "PurchaseDate is required field")]
        public DateTime PurchaseDate { get; set; }
        
        [Required]
        public bool IsReAssignable { get; set; }
    }
}