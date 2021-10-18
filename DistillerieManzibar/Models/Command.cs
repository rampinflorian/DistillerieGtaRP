using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DistillerieManzibar.Enums;

namespace DistillerieManzibar.Models
{
    public class Command
    {
        public int CommandId { get; set; }
        [Required, Display(Name = "Type de liquide")] public LiquidCategory LiquidCategory { get; set; }
        [Required, Display(Name = "Création")] public DateTime CreatedAt { get; set; }
        [Required, Display(Name = "Quantité")] public int Quantity { get; set; }
        [Display(Name = "Payé le")] public DateTime? BilledAt { get; set; }
        [Display(Name = "Livré le")] public DateTime? DeliveryAt { get; set; }
        [Required] public int CompanyId { get; set; }
        [Display(Name = "Société")] public Company Company { get; set; }

        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
        
        
    }
}