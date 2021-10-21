using System.ComponentModel.DataAnnotations;

namespace DistillerieManzibar.Enums
{
    public enum ApplicationRole
    {
        [Display(Name = "Employé")]
        Employee,
        [Display(Name = "Patron")]
        Boss,
        [Display(Name = "Chef d'équipe")]
        Leader,
        [Display(Name = "Co-Patron")]
        CoBoss,
        [Display(Name = "Apprenti")]
        Learner
    }
}