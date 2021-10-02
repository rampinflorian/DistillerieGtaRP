using System.ComponentModel.DataAnnotations;

namespace DistillerieManzibar.Enums
{
    public enum LiquidCategory
    {
        [Display(Name = "sans Alcool")]
        NoAlcool,
        [Display(Name = "avec Alcool")]
        Alcool
    }
}