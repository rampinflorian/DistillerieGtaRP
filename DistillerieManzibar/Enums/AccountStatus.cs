using System.ComponentModel.DataAnnotations;

namespace DistillerieManzibar.Enums
{
    public enum AccountStatus
    {
        [Display(Name = "Actif")]
        Active,
        [Display(Name = "Inactif")]
        Inactive
    }
}