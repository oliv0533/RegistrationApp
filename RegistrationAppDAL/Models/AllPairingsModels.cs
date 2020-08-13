using System;
using System.Collections.Generic;

namespace RegistrationAppDAL.Models
{
    public class AllPairingsModel
    {
        public AllPairingsModel(
            DateTime date)
        {
            Pairings = new List<Pairing>();
            LeftoverUsers = new List<ApplicationUser>();
            Date = date;
            Id = Guid.NewGuid().ToString();
            Levels = new List<string>();
        }
        public string Id { get; set; }
        public List<Pairing> Pairings { get; set; }
        public List<ApplicationUser> LeftoverUsers { get; set; }
        public List<string> Levels { get; set; }
        public DateTime Date { get; set; }
    }
}
