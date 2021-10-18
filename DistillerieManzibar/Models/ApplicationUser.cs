using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DistillerieManzibar.Enums;
using Microsoft.AspNetCore.Identity;

namespace DistillerieManzibar.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Sold { get; set; }
        public string FullName()
        {
            return $"{FirstName} {LastName}";
        }

        [Display(Name = "Taux")]
        public int Percentage { get; set; }
        [Display(Name = "Dernier payement")]
        public DateTime LastPayementAt { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public ICollection<Command> Commands { get; set; }
    }
}