using System;
using System.ComponentModel.DataAnnotations;
using DistillerieManzibar.Enums;

namespace DistillerieManzibar.Models
{
    public class Pricing
    {
        public int PricingId { get; set; }
        [Required] public int CompanyId { get; set; }
        [Display(Name = "Société")]public Company Company { get; set; }
        [Required, Display(Name = "Type de liquide")] public LiquidCategory LiquidCategory { get; set; }
        [Required, Display(Name = "Prix")] public int Price { get; set; }
        [Required] public DateTime CreatedAt { get; set; }
    }
}