using System.ComponentModel.DataAnnotations;

namespace DistillerieGtaRP.Enums
{
    public enum AccountStatus
    {
        [Display(Name = "Actif")]
        Active = 0,
        [Display(Name = "Inactif")]
        Inactive = 1
    }
}