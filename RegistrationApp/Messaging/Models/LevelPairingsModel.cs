using System.Collections.Generic;
using RegistrationAppDAL.Models;

namespace RegistrationApp.Messaging.Models
{
    public class LevelPairingsModel
    {
        public List<PairingModel> Pairings { get; set; }

        public List<UserResponseModel> LeftoverUsers { get; set; }

        public List<string> Levels { get; set; }

        public LevelPairingsModel()
        {
            Pairings = new List<PairingModel>();
            LeftoverUsers = new List<UserResponseModel>();
            Levels = new List<string>();
        }
    }
}