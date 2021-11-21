using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistillerieManzibar.CustomQueryModel
{
    [NotMapped]
    public class BilledCustomQueryModel
    {
        public string ApplicationUserId { get; set; }
        public int Quantity { get; set; }
        public DateTime? LastPayementAt { get; set; }
        public decimal Price { get; set; }
    }
}