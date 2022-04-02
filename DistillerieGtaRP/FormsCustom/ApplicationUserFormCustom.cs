using System.ComponentModel.DataAnnotations;
using DistillerieGtaRP.Enums;
using DistillerieGtaRP.Models;

namespace DistillerieGtaRP.FormsCustom
{
    public class ApplicationUserFormCustom
    {
        public ApplicationUser ApplicationUser { get; set; }
        [Display(Name = "Rôle")]public ApplicationRole? ApplicationRole { get; set; }
    }
}