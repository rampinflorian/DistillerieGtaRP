using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistillerieManzibar.CustomQueryModel
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