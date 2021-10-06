using System.ComponentModel.DataAnnotations;

namespace DistillerieManzibar.Enums
{
    public enum LiquidCategory
    {
        [Display(Name = "Sans alcool")]
        NoAlcool,
        [Display(Name = "Avec alcool")]
        Alcool
    }
}