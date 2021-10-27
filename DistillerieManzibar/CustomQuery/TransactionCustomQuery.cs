using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistillerieManzibar.CustomQuery
{
    [NotMapped]
    public class TransactionCustomQuery
    {
        public DateTime CreatedAt { get; set; }
        public int Quantity { get; set; }
    }
}