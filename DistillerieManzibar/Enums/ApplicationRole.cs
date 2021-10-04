using System.ComponentModel.DataAnnotations;

namespace DistillerieManzibar.Enums
{
    public enum ApplicationRole
    {
        [Display(Name = "Employé")]
        Employee,
        [Display(Name = "Patron")]
        Boss
    }
}