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

        public int Percentage { get; set; }
    }
}