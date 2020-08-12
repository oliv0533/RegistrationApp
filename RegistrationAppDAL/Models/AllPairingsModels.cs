using System;
using System.Collections.Generic;

namespace RegistrationAppDAL.Models
{
    public class AllPairingsModel
    {
        public AllPairingsModel(
            List<ApplicationUser> leftoverUsers, 
            List<Pairing> pairings,
            DateTime date)
        {
            LeftoverUsers = leftoverUsers;
            Pairings = pairings;
            Date = date;
        }

        public List<Pairing> Pairings { get; set; }
        public List<ApplicationUser> LeftoverUsers { get; set; }
        public DateTime Date { get; set; }
    }
}
