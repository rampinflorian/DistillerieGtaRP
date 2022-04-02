using DistillerieGtaRP.Enums;
using DistillerieGtaRP.Models;

namespace DistillerieGtaRP.Services.Stats.Model
{
    public class StatsPricing
    {
        public Company Company { get; set; }
        public LiquidCategory LiquidCategory { get; set; }
        public int Price { get; set; }
    }
}