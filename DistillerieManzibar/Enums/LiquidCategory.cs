using System.ComponentModel.DataAnnotations;

namespace DistillerieManzibar.Enums
{
    public enum LiquidCategory
    {
        [Display(Name = "Eau")]
        NoAlcool,
        [Display(Name = "Alcool")]
        Alcool
    }
}