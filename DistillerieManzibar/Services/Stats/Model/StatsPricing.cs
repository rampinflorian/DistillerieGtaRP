using DistillerieManzibar.Enums;
using DistillerieManzibar.Models;

namespace DistillerieManzibar.Services.Stats.Model
{
    public class StatsPricing
    {
        public Company Company { get; set; }
        public LiquidCategory LiquidCategory { get; set; }
        public int Price { get; set; }
    }
}