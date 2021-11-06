using System.ComponentModel.DataAnnotations;
using DistillerieManzibar.Enums;
using DistillerieManzibar.Models;

namespace DistillerieManzibar.FormsCustom
{
    public class ApplicationUserFormCustom
    {
        public ApplicationUser ApplicationUser { get; set; }
        [Display(Name = "Rôle")]public ApplicationRole? ApplicationRole { get; set; }
    }
}