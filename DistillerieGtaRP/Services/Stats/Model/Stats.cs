using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DistillerieGtaRP.Models;

namespace DistillerieGtaRP.Services.Stats.Model
{
    [NotMapped]
    public class Stats
    {
        public int SumApplicationUser { get; set; }
        public List<Transaction> Harvests { get; set; }
        public List<Command> Commands { get; set; }
        
        public List<StatsPricing> StatsPricingList{ get; set; }
        
    }
}