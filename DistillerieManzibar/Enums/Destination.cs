using System.ComponentModel.DataAnnotations;

namespace DistillerieManzibar.Enums
{
    public enum Destination
    {
        [Display(Name = "Exportation")]
        Export,
        [Display(Name = "Stock")]
        Stock
    }
}