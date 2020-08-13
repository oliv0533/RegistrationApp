using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationApp.Messaging.Models
{
    public class PairingModel
    {
        public PairingModel(UserResponseModel female, UserResponseModel male)
        {
            Female = female;
            Male = male;
        }

        public UserResponseModel Male { get; set; }
        public UserResponseModel Female { get; set; }
    }
}
