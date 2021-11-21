using System.ComponentModel.DataAnnotations;

namespace DistillerieManzibar.Enums
{
    public enum Destination
    {
        [Display(Name = "Exportation")]
        Export = 0,
        [Display(Name = "Stock")]
        Stock = 1
    }
}