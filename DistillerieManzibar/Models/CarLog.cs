using System;
using DistillerieManzibar.Enums;

namespace DistillerieManzibar.Models
{
    public class CarLog
    {
        public int CarLogId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public CarDirection CarDirection { get; set; }
        public DateTime CreatedAt { get; set; }
        public Car Car { get; set; }
    }
}