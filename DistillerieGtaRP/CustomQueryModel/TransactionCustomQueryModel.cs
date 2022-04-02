using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistillerieGtaRP.CustomQueryModel
{
    [NotMapped]
    public class TransactionCustomQueryModel
    {
        public DateTime CreatedAt { get; set; }
        public int Quantity { get; set; }
    }
}