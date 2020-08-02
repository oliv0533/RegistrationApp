using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace RegistrationAppDAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<FormerMatch> FormerMatches { get; }

        public Level Level { get; set; }

        public DanceGender Gender { get; set; }

        public ApplicationUser(Level level, DanceGender gender)
        {
            Level = level;
            Gender = gender;
            FormerMatches = new List<FormerMatch>();
        }

        public DateTime? Attending { get; set; }
    }

    public class FormerMatch
    {
        public string PartnerId { get; }

        public DateTime DateDanced { get;}

        public FormerMatch(string partnerId, DateTime dateDanced)
        {
            PartnerId = partnerId;
            DateDanced = dateDanced;
        }
    }
}