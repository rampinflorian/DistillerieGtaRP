using System.ComponentModel.DataAnnotations;

namespace DistillerieManzibar.Enums
{
    public enum LiquidCategory
    {
        [Display(Name = "Eau")]
        NoAlcool = 0,
        [Display(Name = "Alcool")]
        Alcool = 1
    }
}