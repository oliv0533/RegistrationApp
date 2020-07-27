using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace RegistrationAppDAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<FormerMatches> FormerMatches { get; }

        public Level Level { get; set; }

        public DanceGender Gender { get; set; }

        public ApplicationUser(Level level, DanceGender gender)
        {
            Level = level;
            Gender = gender;
            FormerMatches = new List<FormerMatches>();
        }

        public DateTime? Attending { get; set; }
    }

    public class FormerMatches
    {
        public string PartnerId { get; }

        public DateTime DateDanced { get;}

        public FormerMatches(string partnerId, DateTime dateDanced)
        {
            PartnerId = partnerId;
            DateDanced = dateDanced;
        }
    }
}