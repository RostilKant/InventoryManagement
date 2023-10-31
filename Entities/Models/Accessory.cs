using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Enums;
using Entities.IdentityModels;

namespace Entities.Models
{
    public class Accessory : IMustHaveTenant
    {
        [Column("AccessoryId")]
        public Guid Id { get; set; }
        
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
        
        public Device Device { get; set; }
        
        
        public User User { get; set; }
        
        public string TenantId { get; set; }
    }
}