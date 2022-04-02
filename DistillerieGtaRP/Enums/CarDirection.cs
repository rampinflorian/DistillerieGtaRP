using System.ComponentModel.DataAnnotations;

namespace DistillerieGtaRP.Enums
{
    public enum CarDirection
    {
        [Display(Name = "Pris")]
        Get = 0,
        [Display(Name = "Rendu")]
        Drop = 1,
    }
}