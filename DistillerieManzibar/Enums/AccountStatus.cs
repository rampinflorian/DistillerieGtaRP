using System.ComponentModel.DataAnnotations;

namespace DistillerieManzibar.Enums
{
    public enum AccountStatus
    {
        [Display(Name = "Actif")]
        Active = 0,
        [Display(Name = "Inactif")]
        Inactive = 1
    }
}