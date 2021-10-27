using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistillerieManzibar.CustomQueryModel
{
    [NotMapped]
    public class TransactionCustomQueryModel
    {
        public DateTime CreatedAt { get; set; }
        public int Quantity { get; set; }
    }
}