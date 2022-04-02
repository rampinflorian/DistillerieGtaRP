using System.ComponentModel.DataAnnotations;

namespace DistillerieGtaRP.Enums
{
    public enum ApplicationRole
    {
        [Display(Name = "Employé")]
        Employee = 1,
        [Display(Name = "Patron")]
        Boss = 2,
        [Display(Name = "Chef d'équipe")]
        Leader = 3,
        [Display(Name = "Co-Patron")]
        CoBoss = 4,
        [Display(Name = "Apprenti")]
        Learner = 5,
        [Display(Name = "Gouvernement")]
        Government = 6,
        [Display(Name = "Administration")]
        Administration = 7,
        [Display(Name = "Licencié")]
        Dismissed = 8
        
        
    }
}