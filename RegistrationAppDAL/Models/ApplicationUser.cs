using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace RegistrationAppDAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<string> FormerMatches { get; }

        public DanceClass Level { get; set; }

        public ApplicationUser(DanceClass level)
        {
            Level = level;
            FormerMatches = new List<string>();
        }
    }
}