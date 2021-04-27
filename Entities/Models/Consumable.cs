using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Enums;

namespace Entities.Models
{
    public class Consumable
    {
        [Column("ConsumableId")]
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Status is required field")]
        public AssetStatus Status { get; set; }
        
        [Required(ErrorMessage = "Manufacturer is required field")]
        public string Manufacturer { get; set; }
        
        [Required(ErrorMessage = "PurchaseCost is required field")]
        public decimal PurchaseCost { get; set; }
        
        [Required(ErrorMessage = "PurchaseDate is required field")]
        public DateTime PurchaseDate { get; set; }
        
        public string OfficeAddress { get; set; }

        public Device Device { get; set; }
    }
}