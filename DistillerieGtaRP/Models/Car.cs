using System.ComponentModel.DataAnnotations;

namespace DistillerieGtaRP.Models
{
    public class Car
    {
        public int CarId { get; set; }
        [Required, Display(Name = "Nom")] public string Name { get; set; }
        [Display(Name = "Employé")] public ApplicationUser ApplicationUser { get; set; }
        [Display(Name = "Immatriculation")] public string Registration { get; set; }
    }
}