using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace RegistrationAppDAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<string> FormerMatches { get; }

        public Level Level { get; set; }

        public DanceGender Gender { get; set; }

        public ApplicationUser(Level level, DanceGender gender)
        {
            Level = level;
            Gender = gender;
            FormerMatches = new List<string>();
        }

        public DateTime? Attending { get; set; }
    }
}