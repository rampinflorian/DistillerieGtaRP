using System.ComponentModel.DataAnnotations;

namespace DistillerieManzibar.Enums
{
    public enum CarDirection
    {
        [Display(Name = "Pris")]
        Get,
        [Display(Name = "Rendu")]
        Drop,
    }
}