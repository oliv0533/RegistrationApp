using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace RegistrationAppDAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<FormerMatch> FormerMatches { get; }

        public List<Level> Levels { get; set; }

        public DanceGender Gender { get; set; }

        public Attending? Attending { get; set; }

        public ApplicationUser(List<Level> levels, DanceGender gender)
        {
            Levels = levels;
            Gender = gender;
            FormerMatches = new List<FormerMatch>();
        }
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

    public class Attending : IComparable
    {
        public Attending(List<Level> levels, DateTime date)
        {
            Levels = levels;
            Date = date;
        }

        public DateTime Date { get; }

        public List<Level> Levels { get; }
        public int CompareTo(object? obj)
        {
            if (obj == null)
            {
                return -1;
            }
            Attending a = (Attending) obj;
            if (Date < a.Date) return -1;
            return Date.Equals(a.Date) ? 0 : 1;
        }
    }
}