using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace RegistrationAppDAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<FormerMatch> FormerMatches { get; }

        public List<string> Levels { get; set; }

        public string Gender { get; set; }

        public Attending? Attending { get; set; }

        public ApplicationUser(string gender)
        {
            Levels = new List<string>();
            Gender = gender;
            FormerMatches = new List<FormerMatch>();
        }
    }

    public class FormerMatch
    {
        public string PartnerId { get; set; }

        public DateTime DateDanced { get; set; }

        public string Id { get; set; }

        public FormerMatch(string partnerId, DateTime dateDanced)
        {
            PartnerId = partnerId;
            DateDanced = dateDanced;
            Id = Guid.NewGuid().ToString();
        }
    }

    public class Attending : IComparable
    {
        public Attending(DateTime date)
        {
            Levels = new List<string>();
            Date = date;
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public DateTime Date { get; set; }

        public List<string> Levels { get; set; }
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