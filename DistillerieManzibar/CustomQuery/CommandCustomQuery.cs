using System;
using System.ComponentModel.DataAnnotations.Schema;
using Humanizer;

namespace DistillerieManzibar.CustomQuery
{
    [NotMapped]
    public class CommandCustomQuery
    {
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public int  CompanyId { get; set; }
        public string Name { get; set; }
    }
}