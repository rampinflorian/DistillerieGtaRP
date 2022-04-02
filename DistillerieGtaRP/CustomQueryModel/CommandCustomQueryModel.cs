using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistillerieGtaRP.CustomQueryModel
{
    [NotMapped]
    public class CommandCustomQueryModel
    {
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public int  CompanyId { get; set; }
        public string Name { get; set; }
    }
}