using System.ComponentModel.DataAnnotations;

namespace DistillerieGtaRP.Enums
{
    public enum LiquidCategory
    {
        [Display(Name = "Eau")]
        NoAlcool = 0,
        [Display(Name = "Alcool")]
        Alcool = 1
    }
}