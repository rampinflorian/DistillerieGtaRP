using System;
using System.ComponentModel.DataAnnotations;
using DistillerieManzibar.Enums;

namespace DistillerieManzibar.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        [Required, Display(Name = "Employé")] public ApplicationUser ApplicationUser { get; set; }
        [Required, Display(Name = "Type de liquide")] public LiquidCategory LiquidCategory { get; set; }
        [Required, Display(Name = "Destination")] public Destination Destination { get; set; }
        [Required, Display(Name = "Quantité")] public int Quantity { get; set; }
        [Required, Display(Name = "Date"), DataType(DataType.Date)]public DateTime CreatedAt { get; set; }
    }
}